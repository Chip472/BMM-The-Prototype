using UnityEngine;

public class CheeseDragAndDrop : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isDragging = false;
    private bool isOverGrinder = false;
    private float grindingTime = 0f;

    public Animator grinderAnimator;
    public GameObject cheeseEffect;

    public GameObject grindedCheese;
    private SpriteRenderer grindedRenderer;

    public GameObject grinder; // Assign the grinder GameObject in Inspector

    private Camera cam;

    public AudioSource cheeseSFX;
    void Start()
    {
        cam = Camera.main;
        originalPosition = transform.localPosition;
        grindedRenderer = grindedCheese.GetComponent<SpriteRenderer>();

        if (grindedRenderer != null)
        {
            Color c = grindedRenderer.color;
            c.a = 0f;
            grindedRenderer.color = c;
        }
    }

    private bool wasOverGrinderLastFrame = false;

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;

            CheckIfOverGrinder();

            if (isOverGrinder)
            {
                if (!wasOverGrinderLastFrame)
                {
                    cheeseSFX.Play();
                    cheeseEffect.SetActive(true);
                }

                grindingTime += Time.deltaTime;

                if (grindedRenderer != null)
                {
                    float alpha = Mathf.Clamp01(grindingTime / 4.5f);
                    Color c = grindedRenderer.color;
                    c.a = alpha;
                    grindedRenderer.color = c;
                }
            }
            else
            {
                if (wasOverGrinderLastFrame)
                {
                    cheeseSFX.Stop();
                    cheeseEffect.SetActive(false);
                }
            }

            wasOverGrinderLastFrame = isOverGrinder;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            cheeseEffect.SetActive(false);
            grinderAnimator.Play("GrinderDisappearAnim");
            transform.localPosition = originalPosition;
            GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            isOverGrinder = false;

            // Also stop sound on release
            cheeseSFX.Stop();
            wasOverGrinderLastFrame = false;
        }
    }


    void OnMouseDown()
    {
        isDragging = true;
        grinderAnimator.Play("GrinderAppearAnim");
        GetComponent<SpriteRenderer>().sortingLayerName = "DraggingItem";
    }

    private void CheckIfOverGrinder()
    {
        Collider2D grinderCol = grinder.GetComponent<Collider2D>();
        if (grinderCol != null)
        {
            isOverGrinder = grinderCol.OverlapPoint(transform.position);
        }
    }
}
