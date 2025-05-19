using UnityEngine;

public class IngredientPourReceiver : MonoBehaviour
{
    [Header("Animation Object")]
    public GameObject animatedSpriteObject;

    [Header("Pouring Requirement")]
    public float requiredWeight = 100f;
    public float weightPerParticle = 1f;

    [Header("Next Step Trigger")]
    public PancakeManager manager;         // Script that contains the method
    public string onCompleteMethod;            // Method name to call when step is complete

    private Animator animator;
    private bool isReceivingParticles = false;

    public float currentWeight = 0f;


    private void Awake()
    {
        if (animatedSpriteObject != null)
        {
            animator = animatedSpriteObject.GetComponent<Animator>();
            animatedSpriteObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (animator != null && animatedSpriteObject.activeSelf && !isReceivingParticles)
        {
            animator.speed = 0f;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        isReceivingParticles = true;

        if (!animatedSpriteObject.activeSelf)
        {
            animatedSpriteObject.SetActive(true);
            animator.Play(0, 0, 0f);
        }

        animator.speed = 1f;

        // Increase weight
        currentWeight += weightPerParticle;

        CancelInvoke(nameof(ResetParticleState));
        Invoke(nameof(ResetParticleState), 0.1f);
    }

    private void ResetParticleState()
    {
        isReceivingParticles = false;
    }

    public void TryProceedToNextStep()
    {
        animator.speed = 0f;
        if (manager != null && !string.IsNullOrEmpty(onCompleteMethod))
        {
            Debug.Log("Done");
            Debug.Log(currentWeight);
            manager.Invoke(onCompleteMethod, 0f);
        }
    }

    public void ResetWeight()
    {
        currentWeight = 0f;
    }
}
