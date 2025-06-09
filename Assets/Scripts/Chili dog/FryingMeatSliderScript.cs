using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FryingMeatSliderScript : MonoBehaviour
{
    public Slider cookingSlider;
    public GameObject pepperDrag, pepper;
    public GameObject chilliDrag, chilli;

    public GameObject bowlObject;
    public SpriteRenderer rawMeat, cookedMeat;

    public float decreaseSpeed = 0.2f;
    public float increaseAmount = 0.05f;
    public float perfectZoneMin = 0.58f;
    public float perfectZoneMax = 0.94f;

    public bool isCooking = false;

    private bool timerStarted = false;

    public Animator fryingAnim;
    public GameObject nextPhrase;

    bool isTouchEndpoint = false;

    public AudioSource sizzleSfx, daoThitSFX;

    private void Start()
    {
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(5f);
        pepperDrag.GetComponent<DragAndDropScript>().isActive = true;
        chilliDrag.GetComponent<DragAndDropScript>().isActive = true;
        cookedMeat.gameObject.SetActive(true);
        isCooking = true;
    }

    void Update()
    {

        if (!timerStarted && pepper.activeSelf && chilli.activeSelf && cookedMeat.color.a == 1f)
        {
            timerStarted = true;
            EvaluateCookingResult();
        }

        if (!isCooking) return;

        // Decrease the slider while cooking
        cookingSlider.value -= decreaseSpeed * Time.deltaTime;
        cookingSlider.value = Mathf.Clamp01(cookingSlider.value);
        OnPanClicked();

        if (cookingSlider.value == 0f && !isTouchEndpoint)
        {
            isTouchEndpoint = true;
            PlayerPrefs.SetFloat("chap2Score", PlayerPrefs.GetFloat("chap2Score", 0) - 0.1f);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0f;
        return mousePoint;
    }

    public void OnPanClicked()
    {
        if (!isCooking || bowlObject == null) return;

        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Collider2D bowlCol = bowlObject.GetComponent<Collider2D>();
            if (bowlCol != null)
            {
                Vector3 mousePos = GetMouseWorldPos();
                if (bowlCol.OverlapPoint(mousePos))
                {
                    daoThitSFX.Play();
                    isTouchEndpoint = false;

                    // Do effects only on click over bowl
                    cookingSlider.value += increaseAmount;
                    cookingSlider.value = Mathf.Clamp01(cookingSlider.value);

                    rawMeat.flipX = !rawMeat.flipX;
                    cookedMeat.flipX = !cookedMeat.flipX;

                    Color currentColor = cookedMeat.color;
                    cookedMeat.color = new Color(1f, 1f, 1f, Mathf.Clamp01(currentColor.a + 0.1f));
                }
            }
        }
    }


    private void EvaluateCookingResult()
    {
        StartCoroutine(DelayEndScene());

        if (cookingSlider.value >= perfectZoneMin && cookingSlider.value <= perfectZoneMax)
        {

        }
        else
        {
            PlayerPrefs.SetFloat("chap2Score", PlayerPrefs.GetFloat("chap2Score", 0) - 0.1f);
        }
    }

    IEnumerator DelayEndScene()
    {
        yield return new WaitForSeconds(3f);
        sizzleSfx.Stop();
        isCooking = false;
        fryingAnim.SetBool("step5", true);
        GetComponent<Animator>().SetBool("off", true);

        Debug.Log(PlayerPrefs.GetFloat("chap2Score", 0));

        yield return new WaitForSeconds(1.5f);
        nextPhrase.SetActive(true);
        fryingAnim.gameObject.SetActive(false);
    }
}
