using UnityEngine;

public class DragAndDropScaleScript : MonoBehaviour
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

    public Animator bowlNscaleAnim;
    public IngredientPourReceiver receiver;

    public AudioSource ingreSound;

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
        ingreSound.Play();
        if (bowlNscaleAnim != null)
        {
            bowlNscaleAnim.SetBool("isMeasuring", true);
        }

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
            transform.position = GetMouseWorldPos() + offset;
    }

    void OnMouseUp()
    {
        ingreSound.Stop();
        // Try to proceed only if enough weight was poured
        if (receiver.currentWeight >= receiver.requiredWeight - 10f)
        {
            receiver.TryProceedToNextStep();
        }

        if (bowlNscaleAnim != null)
        {
            bowlNscaleAnim.SetBool("isMeasuring", false);
        }

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

}
