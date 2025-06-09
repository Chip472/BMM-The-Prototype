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

    public GameObject onions;

    public AudioSource sizzleSFX;
    bool isPoured = false;

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

        if (isPoured)
        {
            isPoured = false;
            sizzleSFX.Play();
        }
    }

    bool check = false;

    private void OnParticleCollision(GameObject other)
    {
        if (!check)
        {
            check = true;
            isPoured = true;
        }
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
            onions.SetActive(true);
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
