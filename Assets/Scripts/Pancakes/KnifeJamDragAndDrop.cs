using UnityEngine;

public class KnifeJamDragAndDrop : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isDragging = false;

    [Header("Target Bowl Settings")]
    public string bowlTag = "Bowl";
    public GameObject ingredientInBowl;
    public GameObject ingredientInBowl2;

    public GameObject shadow;

    public GameObject jamOnKnife;
    bool check = false;

    public Animator pancakeAnim;
    int pancakeNum = 0;

    public PancakeManager2 manager;

    public AudioSource knifeSound, jam1Sound, jam2Sound;

    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    private void OnMouseDown()
    {
        knifeSound.Play();
        isDragging = true;
        shadow.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "DraggingItem";
        jamOnKnife.GetComponent<SpriteRenderer>().sortingLayerName = "DraggingItem";
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f; // Set to a constant value where the object is visible (e.g., 10 units in front of the camera)
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
            transform.rotation = Quaternion.Euler(0f, 0f, 65f);

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
            foreach (Collider2D hit in hits)
            {
                if (!check && hit.CompareTag("Jam"))
                {
                    check = true;
                    jamOnKnife.SetActive(true);
                    jam1Sound.Play();
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    jamOnKnife.GetComponent<SpriteRenderer>().sortingOrder = 4;
                }
            }
        }
    }


    private void OnMouseUp()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        jamOnKnife.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        isDragging = false;
        shadow.SetActive(true);
        check = false;

        // Create a small radius around the object's center to check for colliders
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (Collider2D hit in hits)
        {
            if (jamOnKnife.activeSelf && hit.CompareTag(bowlTag))
            {
                jam2Sound.Play();
                jamOnKnife.SetActive(false);
                ingredientInBowl.SetActive(true);

                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                jamOnKnife.GetComponent<SpriteRenderer>().sortingOrder = 1;

                pancakeNum++;

                if (pancakeNum == 1)
                {
                    ingredientInBowl = ingredientInBowl2;
                    pancakeAnim.SetBool("pancake2", true);
                }
                else if (pancakeNum == 2)
                {
                    pancakeAnim.SetBool("pancake3", true);
                    manager.ChangeToStep2();
                }
            }
        }

        // If no valid bowl found, return to original position
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;

        if (pancakeNum == 2)
        {
            GetComponent<KnifeJamDragAndDrop>().enabled = false;
        }
    }
}
