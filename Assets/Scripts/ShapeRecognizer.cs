using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using System.IO;

public class ShapeRecognizer : MonoBehaviour
{
    public RectTransform drawAreaUI;
    public GameObject gestureLinePrefab;
    public string expectedShape = "circle"; // this should match current gameobject's shape
    public float accuracyThreshold = 0.8f;

    public ChantingScript chanting;

    private List<Gesture> trainingSet = new List<Gesture>();
    private List<Point> points = new List<Point>();
    private LineRenderer currentLine;
    private int strokeId = -1;
    private Vector3 virtualKeyPosition;
    private int vertexCount = 0;

    void Start()
    {
        // Load saved gestures
        string path = Application.streamingAssetsPath + "/SavedShapes/";
        foreach (string file in Directory.GetFiles(path, "*.xml"))
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(file));
        }
    }

    void Update()
    {
        UpdateVirtualKeyPosition();

        if (Input.GetMouseButtonDown(0))
        {
            if (IsWithinDrawArea(virtualKeyPosition))
            {
                StartDrawing();
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (currentLine != null && IsWithinDrawArea(virtualKeyPosition))
            {
                DrawPoint();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentLine != null)
            {
                RecognizeShape();
                ClearDrawing();
            }
        }
    }

    void UpdateVirtualKeyPosition()
    {
        if (Application.isMobilePlatform && Input.touchCount > 0)
            virtualKeyPosition = Input.GetTouch(0).position;
        else if (Input.GetMouseButton(0))
            virtualKeyPosition = Input.mousePosition;
    }

    void StartDrawing()
    {
        strokeId++;
        points.Clear();

        GameObject lineObj = Instantiate(gestureLinePrefab);
        currentLine = lineObj.GetComponent<LineRenderer>();
        currentLine.positionCount = 0;
        vertexCount = 0;
    }

    void DrawPoint()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10));
        points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

        currentLine.positionCount = ++vertexCount;
        currentLine.SetPosition(vertexCount - 1, worldPos);
    }

    public void SetShapeName(string name)
    {
        if (name != null)
        {
            expectedShape = name;
        }
    }

    void RecognizeShape()
    {
        Gesture candidate = new Gesture(points.ToArray());
        Result result = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

        Debug.Log("Recognized: " + result.GestureClass + " | Score: " + result.Score);

        if (result.GestureClass == expectedShape && result.Score >= accuracyThreshold)
        {
            chanting.Success();
        }
        else
        {
            chanting.Fail();
        }
    }

    void ClearDrawing()
    {
        strokeId = -1;
        points.Clear();

        if (currentLine != null)
        {
            Destroy(currentLine.gameObject);
            currentLine = null;
        }
    }

    bool IsWithinDrawArea(Vector3 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            drawAreaUI, screenPos, null, out Vector2 localPoint);
        return drawAreaUI.rect.Contains(localPoint);
    }
}
