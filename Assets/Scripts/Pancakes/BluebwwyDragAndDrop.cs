using UnityEngine;

public class BluebwwyDragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    private SpriteRenderer spriteRenderer;

    [Header("Bowl Settings")]
    public string bowlTag = "Bowl";
    public GameObject ingredientInBowl;
    public string bbname;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Transform parent = transform.parent;
        if (parent != null)
        {
            Transform child = parent.Find(bbname);
            if (child != null)
                ingredientInBowl = child.gameObject;
        }
    }

    private bool isDraggingExternally = false;

    public void BeginDrag(Vector3 mouseWorldPos)
    {
        isDraggingExternally = true;
        spriteRenderer.sortingLayerName = "DraggingItem";
        offset = transform.position - new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);
    }

    private void Update()
    {
        if (isDraggingExternally && Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z) + offset;
        }

        // If mouse released, stop dragging
        if (isDraggingExternally && Input.GetMouseButtonUp(0))
        {
            isDraggingExternally = false;
            OnMouseUp();
        }
    }


    private void OnMouseUp()
    {
        spriteRenderer.sortingLayerName = "Default";

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f); // bigger radius
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag(bowlTag))
            {
                if (ingredientInBowl != null)
                {
                    ingredientInBowl.SetActive(true);
                    if (GetComponent<AudioSource>() != null){

                        GetComponent<AudioSource>().Play();
                    }
                }

                Destroy(gameObject);
                return;
            }
        }

        Destroy(gameObject); // No bowl found
    }

}
