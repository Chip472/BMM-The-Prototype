using UnityEngine;

public class DragAndDropOil : MonoBehaviour
{
    public Sprite dragSprite;
    public GameObject shadow;
    public ParticleSystem[] dragParticles;

    private Sprite originalSprite;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 originalPosition;

    public OilReceiver receiver;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        originalSprite = spriteRenderer.sprite;
        originalPosition = transform.localPosition;

        if (shadow != null)
            shadow.SetActive(true);

        foreach (var ps in dragParticles)
            if (ps != null)
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void OnMouseDown()
    {

        isDragging = true;
        offset = transform.position - GetMouseWorldPos();

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        spriteRenderer.sortingLayerName = "DraggingItem";
        spriteRenderer.sprite = dragSprite;
        UpdateColliderToMatchSprite();

        if (shadow != null)
            shadow.SetActive(false);

        foreach (var ps in dragParticles)
            if (ps != null)
            {
                ps.Play();
            }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    void OnMouseUp()
    {

        isDragging = false;
        rb.bodyType = RigidbodyType2D.Dynamic;

        spriteRenderer.sortingLayerName = "Default";
        spriteRenderer.sprite = originalSprite;
        UpdateColliderToMatchSprite();

        if (shadow != null)
            shadow.SetActive(true);

        foreach (var ps in dragParticles)
            if (ps != null)
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        transform.localPosition = originalPosition;
        rb.linearVelocity = Vector2.zero;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0;
        return mousePoint;
    }

    void UpdateColliderToMatchSprite()
    {
        PolygonCollider2D oldCollider = GetComponent<PolygonCollider2D>();
        if (oldCollider != null)
        {
            Destroy(oldCollider);
            gameObject.AddComponent<PolygonCollider2D>();
        }
    }
    public void ForceReset()
    {
        isDragging = false;
        rb.bodyType = RigidbodyType2D.Dynamic;

        spriteRenderer.sortingLayerName = "Default";
        spriteRenderer.sprite = originalSprite;
        UpdateColliderToMatchSprite();

        if (shadow != null)
            shadow.SetActive(true);

        foreach (var ps in dragParticles)
            if (ps != null)
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        transform.localPosition = originalPosition;
        rb.linearVelocity = Vector2.zero;
    }

}
