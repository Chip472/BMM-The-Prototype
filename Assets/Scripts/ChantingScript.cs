using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChantingScript : MonoBehaviour
{
    [System.Serializable]
    public class ShapePrefab
    {
        public string shapeName;
        public GameObject prefab;
    }

    public List<ShapePrefab> shapePrefabs;
    public Transform spawnPoint;
    public GameObject continueButton;

    public string foodName;
    public GameObject kirakira;
    public GameObject overlay;

    private Queue<string> shapeSequence = new Queue<string>();
    private Dictionary<string, GameObject> shapeDictionary;
    private GameObject currentShapeInstance;
    private string currentExpectedShape;

    private void Start()
    {
        shapeDictionary = new Dictionary<string, GameObject>();
        foreach (var shape in shapePrefabs)
        {
            shapeDictionary[shape.shapeName] = shape.prefab;
        }

        continueButton.SetActive(false);

        // Example sequence: 3 random from triangle/square/circle, 3 from spiral/infinity, then 2 stars
        List<string> sequence = GenerateCustomSequence(foodName);
        foreach (string s in sequence)
            shapeSequence.Enqueue(s);

        SpawnNextShape();
    }

    List<string> GenerateCustomSequence(string foodName)
    {
        List<string> result = new List<string>();

        string[] group1 = { "triangle", "square", "circle" };
        string[] group2 = { "spiral", "infinity" };

        if (foodName == "pancake")
        {
            for (int i = 0; i < 3; i++)
                result.Add(group1[Random.Range(0, group1.Length)]);
        }
        else if (foodName == "chilidog")
        {
            for (int i = 0; i < 3; i++)
                result.Add(group1[Random.Range(0, group1.Length)]);

            for (int i = 0; i < 3; i++)
                result.Add(group2[Random.Range(0, group2.Length)]);
        }

        result.Add("star");
        result.Add("star");

        return result;
    }

    void SpawnNextShape()
    {
        if (shapeSequence.Count == 0)
        {
            continueButton.SetActive(true);
            FindFirstObjectByType<ShapeRecognizer>().gameObject.SetActive(false);

            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            kirakira.SetActive(false);
            return;
        }

        string shapeName = shapeSequence.Dequeue();
        currentExpectedShape = shapeName;

        if (currentShapeInstance != null)
            Destroy(currentShapeInstance);

        GameObject prefab = shapeDictionary[shapeName];
        currentShapeInstance = Instantiate(prefab, spawnPoint.position, Quaternion.identity, spawnPoint);

        // Sync with P-dollar system
        FindFirstObjectByType<ShapeRecognizer>().SetShapeName(shapeName);
        overlay.SetActive(true);
    }

    bool check = false;
    public void Success()
    {
        // Play success animation and proceed
        if (!check)
        {
            check = true;
            GetComponent<PancakeManager2>().cookIntro.NextLine();
            StartCoroutine(WaitForDialogue());
        }

        StartCoroutine(SuccessRoutine());
    }

    IEnumerator WaitForDialogue()
    {
        yield return new WaitForSeconds(4f);
        GetComponent<PancakeManager2>().cookIntro.NextLine();
    }

    IEnumerator SuccessRoutine()
    {
        overlay.GetComponent<Animator>().SetBool("done", true);

        Animator anim = currentShapeInstance.GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("chant", true);

        yield return new WaitForSeconds(1f);

        overlay.SetActive(false);
        Destroy(currentShapeInstance);
        SpawnNextShape();
    }

    public void Fail()
    {
        StartCoroutine(FailRoutine());
    }

    IEnumerator FailRoutine()
    {

        Animator anim = currentShapeInstance.GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("fail", true);

        yield return new WaitForSeconds(0.5f);

        if (anim != null)
            anim.SetBool("fail", false);

    }

    public void OnContinueButtonPressed()
    {
        // Change to next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
