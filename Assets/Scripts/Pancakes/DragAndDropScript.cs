using UnityEngine;

public class DragAndDropScript : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isDragging = false;

    [Header("Target Bowl Settings")]
    public string bowlTag = "Bowl"; 
    public GameObject ingredientInBowl;
    public GameObject shadow;

    public Sprite oriSpr, draggingSpr;

    public bool isInactiveAfterUsed = false;

    public AudioSource ingreSound;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        shadow.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "DraggingItem";

        if (draggingSpr != null || oriSpr != null)
        {
            GetComponent<SpriteRenderer>().sprite = draggingSpr;
        }
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f; // Set to a constant value where the object is visible (e.g., 10 units in front of the camera)
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }


    private void OnMouseUp()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        isDragging = false;
        shadow.SetActive(true);

        // Create a small radius around the object's center to check for colliders
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag(bowlTag))
            {
                ingreSound.Play();
                ingredientInBowl.SetActive(true);
                if (isInactiveAfterUsed)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        // If no valid bowl found, return to original position
        transform.localPosition = originalPosition;
        if (draggingSpr != null || oriSpr != null)
        {
            GetComponent<SpriteRenderer>().sprite = oriSpr;
        }
    }

}
