using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KnifeCuttingController : MonoBehaviour
{
    public Transform knife;
    private Vector3 knifeStartPos;
    public float minSwipeDistance = 100f;

    public enum SwipeDirection { Up, Down, Left, Right }
    public SwipeDirection currentExpectedDirection;

    public GameObject[] onionCutStages; // 0: full, 1~5: each slice
    public GameObject[] onion2CutStages; // 0: full, 1~5: each slice
    public GameObject[] onion3CutStages;

    public GameObject[] sausageCutStages; // 0: full, 1~4: each slice
    public int phrase = 0;

    public Animator arrowAnim;

    public GameObject fryingScene;

    private Vector2 swipeStart;
    private bool isDragging = false;
    private int currentCutStep = 0;

    private void Start()
    {
        ShowNextArrow();
        knifeStartPos = knife.localPosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            arrowAnim.gameObject.SetActive(false);
            swipeStart = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            knife.position = new Vector3(mousePos.x, mousePos.y, knife.position.z);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;

            // Reset knife position
            knife.localPosition = knifeStartPos;

            Vector2 swipeEnd = Input.mousePosition;
            Vector2 swipeVector = swipeEnd - swipeStart;

            if (swipeVector.magnitude >= minSwipeDistance)
            {
                SwipeDirection direction = GetSwipeDirection(swipeVector);
                if (direction == currentExpectedDirection)
                {
                    ProceedCutStep();
                }
            }

        }
    }

    SwipeDirection GetSwipeDirection(Vector2 swipe)
    {
        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            return swipe.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        }
        else
        {
            return swipe.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
        }
    }

    void ProceedCutStep()
    {
        if (phrase == 0 && currentCutStep < onionCutStages.Length - 1)
        {
            onionCutStages[currentCutStep].SetActive(false);
            onion2CutStages[currentCutStep].SetActive(true);
            currentCutStep++;
            onionCutStages[currentCutStep].SetActive(true);

            if (currentCutStep == onionCutStages.Length - 1)
            {
                phrase = 1;
                currentCutStep = 0;
                ReverseArray<GameObject>(onion2CutStages);
            }

            ShowNextArrow();
        }
        else if (phrase == 1 && currentCutStep < onion2CutStages.Length)
        {
            onion2CutStages[currentCutStep].SetActive(false);
            onion3CutStages[currentCutStep].SetActive(true);
            currentCutStep++;

            if (currentCutStep == onion2CutStages.Length)
            {
                StartCoroutine(DelayOutro());
                return;
            }
            else
            {
                onion2CutStages[currentCutStep].SetActive(true);
            }

            ShowNextArrow();
        }
        else if (phrase == 2 && currentCutStep < sausageCutStages.Length - 1)
        {
            sausageCutStages[currentCutStep].SetActive(false);
            currentCutStep++;
            sausageCutStages[currentCutStep].SetActive(true);
            ShowNextArrow();
        }
    }

    IEnumerator DelayOutro()
    {
        yield return new WaitForSeconds(0.8f);

        gameObject.GetComponent<Animator>().SetBool("change", true);

        yield return new WaitForSeconds(2f);
        fryingScene.SetActive(true);
        gameObject.SetActive(false);
    }

    void ShowNextArrow()
    {
        if (phrase == 0)
        {
            currentExpectedDirection = Random.Range(0, 2) == 0 ? SwipeDirection.Up : SwipeDirection.Down;
        }
        else
        {
            currentExpectedDirection = Random.Range(0, 2) == 0 ? SwipeDirection.Left : SwipeDirection.Right;
        }

        SetArrowDirection(currentExpectedDirection);
    }

    void SetArrowDirection(SwipeDirection dir)
    {
        arrowAnim.gameObject.SetActive(true);

        switch (dir)
        {
            case SwipeDirection.Up:
                arrowAnim.Play("ArrowUpAnim", 0);
                break;
            case SwipeDirection.Down:
                arrowAnim.Play("ArrowDownAnim", 0);
                break;
            case SwipeDirection.Left:
                arrowAnim.Play("ArrowLeftAnim", 0);
                break;
            case SwipeDirection.Right:
                arrowAnim.Play("ArrowRightAnim", 0);
                break;
        }
    }

    public static void ReverseArray<T>(T[] array)
    {
        int left = 0;
        int right = array.Length - 1;

        while (left < right)
        {
            T temp = array[left];
            array[left] = array[right];
            array[right] = temp;

            left++;
            right--;
        }
    }
}
