using UnityEngine;

public class SpatulaMeatDragAndDrop : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isDragging = false;

    [Header("Target Bowl Settings")]
    public string bowlTag = "Bowl";
    public GameObject ingredientInBowl;
    public GameObject ingredientInBowl2;
    public GameObject ingredientInBowl3;

    //public GameObject shadow;

    public GameObject meatOnSpatula;
    bool check = false;

    int meatNum = 0;

    public ChiliDogManager manager;

    public AudioSource meatSound;
    public ChiliDogTutor tutorText;

    private void Start()
    {
        tutorText.NextLine();

        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "DraggingItem";
        meatOnSpatula.GetComponent<SpriteRenderer>().sortingLayerName = "DraggingItem";
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f; // Set to a constant value where the object is visible (e.g., 10 units in front of the camera)
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
            transform.rotation = Quaternion.Euler(0f, 0f, 88f);

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
            foreach (Collider2D hit in hits)
            {
                if (!check && hit.CompareTag("Jam"))
                {
                    check = true;
                    meatOnSpatula.SetActive(true);
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    meatOnSpatula.GetComponent<SpriteRenderer>().sortingOrder = 4;
                }
            }
        }
    }


    private void OnMouseUp()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        meatOnSpatula.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        isDragging = false;
        check = false;

        // Create a small radius around the object's center to check for colliders
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (Collider2D hit in hits)
        {
            if (meatOnSpatula.activeSelf && hit.CompareTag(bowlTag))
            {
                if (meatSound != null)
                {
                    meatSound.Play();
                }
                meatOnSpatula.SetActive(false);
                ingredientInBowl.SetActive(true);

                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                meatOnSpatula.GetComponent<SpriteRenderer>().sortingOrder = 1;

                meatNum++;

                if (meatNum == 1)
                {
                    ingredientInBowl = ingredientInBowl2;
                }
                else if (meatNum == 2)
                {
                    ingredientInBowl = ingredientInBowl3;
                }
                else if (meatNum == 3)
                {
                    manager.ChangeToStep2();
                }
            }
        }

        // If no valid bowl found, return to original position
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;

        if (meatNum == 3)
        {
            GetComponent<SpatulaMeatDragAndDrop>().enabled = false;
        }
    }
}
