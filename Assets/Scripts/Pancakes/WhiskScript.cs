using UnityEngine;
using UnityEngine.UI;

public class WhiskScript : MonoBehaviour
{
    public string bowlTag = "Bowl";
    public GameObject bowlObject;
    public GameObject shadow;
    public Animator mixtureAnimator;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isDragging = false;
    private Vector3 offset;
    private bool isOverBowl = false;

    public AudioSource ingreSound;
    public bool isDone = false;

    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        if (mixtureAnimator != null)
        {
            mixtureAnimator.gameObject.SetActive(true);
            mixtureAnimator.Play(0, 0, 0); // Reset animation
            mixtureAnimator.speed = 0f;    // Pause at start
        }
    }
    

    private void OnMouseDown()
    {
        ingreSound.Play();
        shadow.SetActive(false);
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void OnMouseUp()
    {
        ingreSound.Stop();
        shadow.SetActive(true);
        isDragging = false;
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }

    private void Update()
    {
        CheckIfOverBowl();

        if (mixtureAnimator != null)
        {
            if (isOverBowl)
            {
                mixtureAnimator.speed = 1f;
            }
            else
            {
                mixtureAnimator.speed = 0f;
            }
        }

        if (isDone)
        {
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
        }
    }

    private void CheckIfOverBowl()
    {
        if (bowlObject == null) return;

        Collider2D bowlCol = bowlObject.GetComponent<Collider2D>();
        if (bowlCol != null)
        {
            isOverBowl = bowlCol.OverlapPoint(transform.position);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0f;
        return mousePoint;
    }

}
