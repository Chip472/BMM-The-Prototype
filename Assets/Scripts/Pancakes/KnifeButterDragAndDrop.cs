using UnityEngine;

public class KnifeButterDragAndDrop : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isDragging = false;

    [Header("Target Bowl Settings")]
    public string bowlTag = "Bowl";
    public GameObject ingredientInBowl;

    public GameObject shadow;

    public GameObject butterOnKnife1, butterOnKnife2;
    bool check = false;

    public Animator decorAnim;

    public AudioSource knifeSFX, butter1SFX, butter2SFX;

    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    private void OnMouseDown()
    {
        knifeSFX.Play();
        isDragging = true;
        shadow.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "DraggingItem";
        butterOnKnife1.GetComponent<SpriteRenderer>().sortingLayerName = "DraggingItem";
        butterOnKnife2.GetComponent<SpriteRenderer>().sortingLayerName = "DraggingItem";
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
                    butterOnKnife1.SetActive(true);
                    butter1SFX.Play();

                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    butterOnKnife1.GetComponent<SpriteRenderer>().sortingOrder = 4;
                    butterOnKnife2.GetComponent<SpriteRenderer>().sortingOrder = 5;
                }
            }
        }
    }


    private void OnMouseUp()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        butterOnKnife1.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        butterOnKnife2.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        isDragging = false;
        shadow.SetActive(true);
        check = false;

        // Create a small radius around the object's center to check for colliders
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (Collider2D hit in hits)
        {
            if (butterOnKnife1.activeSelf && hit.CompareTag(bowlTag))
            {
                butter2SFX.Play();
                butterOnKnife1.SetActive(false);
                ingredientInBowl.SetActive(true);

                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                butterOnKnife1.GetComponent<SpriteRenderer>().sortingOrder = 1;
                butterOnKnife2.GetComponent<SpriteRenderer>().sortingOrder = 1;


                decorAnim.SetBool("step4", true);
            }
        }

        // If no valid bowl found, return to original position
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }
}
