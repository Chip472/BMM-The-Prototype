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

    public GameObject particleS;

    public bool isActive = true;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void OnMouseDown() //Thực hiện ngay khi chuột vừa được bấm xuống
    {
        if (!isActive)
        {
            return;
        }

        isDragging = true;
        if (shadow != null) 
        {
            shadow.SetActive(false);
        }
        if (particleS != null)
        {
            particleS.SetActive(true);
        }
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "DraggingItem";

        if (draggingSpr != null || oriSpr != null)
        {
            GetComponent<SpriteRenderer>().sprite = draggingSpr; //Đổi sprite tĩnh thành sprite đang được kéo
        }
    }
    private void OnMouseDrag() //Thực hiện trong khi người chơi đang giữ chuột
    {
        if (!isActive)
        {
            return;
        }

        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            transform.position = Camera.main.ScreenToWorldPoint(mousePos); //Di chuyển vật theo con trỏ chuột
        }
    }
    private void OnMouseUp() //Thực hiện khi chuột vừa được nhả ra
    {
        if (!isActive)
        {
            return;
        }

        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        isDragging = false;
        if (shadow != null)
        {
            shadow.SetActive(true);
        }

        if (particleS != null)
        {
            particleS.SetActive(false);
        }
        // Create a small radius around the object's center to check for colliders
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag(bowlTag))
            {
                if (ingreSound != null)
                {
                    ingreSound.Play();
                }
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
