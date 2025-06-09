using UnityEngine;
using UnityEngine.UI;

public class SpatulaOnionScript : MonoBehaviour
{
    public GameObject[] onionObjects; // 5 onion GameObjects with Animator
    private Animator[] onionAnimators;

    public GameObject bowlObject; // Assign bowl object with collider
    public GameObject colliderUp;

    public Animator fryingAnim;

    public GameObject oilBottle, oilCollider;

    private Vector3 offset;
    private Vector3 originalPosition;
    private bool isDragging = false;
    private bool isOverlappingBowl = false;
    private bool stepStarted = false;

    public ChiliDogTutor tutorText;
    private void Awake()
    {
        tutorText.NextLine();
        originalPosition = transform.localPosition;

        onionAnimators = new Animator[onionObjects.Length];
        for (int i = 0; i < onionObjects.Length; i++)
        {
            if (onionObjects[i] != null)
                onionAnimators[i] = onionObjects[i].GetComponent<Animator>();
        }

        // Pause all onion animations at start
        SetOnionAnimatorsSpeed(0f);
    }

    private void OnMouseDown()
    {
        if (!stepStarted)
        {
            stepStarted = true;
            colliderUp.SetActive(true);
        }

        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        transform.localPosition = originalPosition;
    }

    private void Update()
    {
        CheckIfOverBowl();

        SetOnionAnimatorsSpeed(isOverlappingBowl ? 1f : 0f);

        if (isOverlappingBowl && AllAnimationsFinished())
        {
            ProceedToNextStep();
            isOverlappingBowl = false; // prevent multiple triggers
        }
    }

    private void CheckIfOverBowl()
    {
        if (bowlObject == null) return;

        Collider2D bowlCol = bowlObject.GetComponent<Collider2D>();
        if (bowlCol != null)
        {
            isOverlappingBowl = bowlCol.OverlapPoint(transform.position);
        }
    }

    private float currentAnimSpeed = -1f;

    private void SetOnionAnimatorsSpeed(float speed)
    {
        if (Mathf.Approximately(currentAnimSpeed, speed)) return; // Avoid unnecessary changes
        currentAnimSpeed = speed;

        foreach (var animator in onionAnimators)
        {
            if (animator != null)
                animator.speed = speed;
        }
    }

    private bool AllAnimationsFinished()
    {
        foreach (var animator in onionAnimators)
        {
            if (animator != null)
            {
                var state = animator.GetCurrentAnimatorStateInfo(0);
                if (state.normalizedTime < 1f || animator.IsInTransition(0))
                    return false;
            }
        }
        return true;
    }

    private void ProceedToNextStep()
    {
        // Snap spatula back to original position and cancel dragging
        isDragging = false;
        transform.localPosition = originalPosition;

        // Disable oil step, enable spice
        oilBottle.SetActive(false);
        oilCollider.SetActive(false);

        // Play frying animation
        fryingAnim.SetBool("step3", true);

        // Disable onion colliders to prevent re-triggering
        for (int i = 0; i < onionObjects.Length; i++)
        {
            if (onionObjects[i] != null)
            {
                onionObjects[i].GetComponent<Collider2D>().enabled = false;
                onionObjects[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                SetOnionAnimatorsSpeed(1f);
            }
        }
    }



    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0f;
        return mousePoint;
    }
}
