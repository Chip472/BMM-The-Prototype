using UnityEngine;

public class OilReceiver : MonoBehaviour
{
    public GameObject animatedSpriteObject;
    public Animator fryingAnim;

    private Animator animator;
    private bool isReceivingParticles = false;

    public DragAndDropOil oilBottle; // Assign in inspector or find dynamically

    private bool hasFinishedOilRising = false;

    public int switchIng = 0;
    public OilReceiver otherSpice;
    public bool isReady = false;

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

        // Check if animation has finished
        if (!hasFinishedOilRising && animatedSpriteObject.activeSelf)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0))
            {
                hasFinishedOilRising = true;
                ProceedToNextStep();
            }
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


        CancelInvoke(nameof(ResetParticleState));
        Invoke(nameof(ResetParticleState), 0.1f);
    }

    private void ResetParticleState()
    {
        isReceivingParticles = false;
    }


    private void ProceedToNextStep()
    {
        if (switchIng == 0 && otherSpice == null) //oil
        {
            fryingAnim.SetBool("step2", true);
            oilBottle.ForceReset();
        }
        else
        {
            isReady = true;
            if (otherSpice.isReady)
            {
                fryingAnim.SetBool("step4", true);
                oilBottle.ForceReset();
            }
        }
    }
}
