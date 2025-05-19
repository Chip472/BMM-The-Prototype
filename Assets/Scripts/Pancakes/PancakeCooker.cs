using UnityEngine;
using UnityEngine.UI;

public class PancakeCooker : MonoBehaviour
{
    public Slider cookSlider;
    public GameObject pancake;

    public Sprite uncookedSprite;
    public Sprite cookedSprite;
    public Sprite burntSprite;

    public float cookSpeed = 20f; // Increase per second
    private bool isCooking = true;

    private SpriteRenderer pancakeSpr;

    public Animator panAnim;
    bool check = false;

    int count = 0;

    public SpriteRenderer[] nextPancakes;
    public SpriteRenderer[] miniPancakes;

    void Start()
    {
        pancakeSpr = pancake.GetComponent<SpriteRenderer>();
        cookSlider.value = 0;
    }

    void Update()
    {
        if (isCooking && cookSlider.value < cookSlider.maxValue)
        {
            cookSlider.value += cookSpeed * Time.deltaTime;
        }
    }

    public void FlipPancake()
    {
        if (!check)
        {
            count++;
            check = true;
            isCooking = false;
            panAnim.SetBool("flip", true);
        }
    }

    public void ChangePancakeSpr()
    {

        float value = cookSlider.value;

        if (value < 60f)
        {
            pancakeSpr.sprite = uncookedSprite;
        }
        else if (value <= 75f)
        {
            pancakeSpr.sprite = cookedSprite;
        }
        else
        {
            pancakeSpr.sprite = burntSprite;
        }
    }

    public void FinishFlip()
    {
        check = false;
        panAnim.SetBool("flip", false);
        isCooking = true;


        if (count < 2)
        {
            cookSlider.value = 0;
            pancakeSpr.sprite = uncookedSprite;
        }
        else if (count == 2)
        {
            miniPancakes[PlayerPrefs.GetInt("cookedPancake")].color = Color.white;

            if (pancakeSpr.sprite == uncookedSprite)
            {
                nextPancakes[PlayerPrefs.GetInt("cookedPancake")].sprite = uncookedSprite;
                miniPancakes[PlayerPrefs.GetInt("cookedPancake")].sprite = uncookedSprite;
            }
            else if (pancakeSpr.sprite == cookedSprite)
            {
                nextPancakes[PlayerPrefs.GetInt("cookedPancake")].sprite = cookedSprite;
                miniPancakes[PlayerPrefs.GetInt("cookedPancake")].sprite = cookedSprite;
            }
            else
            {
                nextPancakes[PlayerPrefs.GetInt("cookedPancake")].sprite = burntSprite;
                miniPancakes[PlayerPrefs.GetInt("cookedPancake")].sprite = burntSprite;
            }

            if (PlayerPrefs.GetInt("cookedPancake", 0) != 2)
            {
                cookSlider.value = 0;
                pancakeSpr.sprite = uncookedSprite;
            }

            count = 0;
            PlayerPrefs.SetInt("cookedPancake", PlayerPrefs.GetInt("cookedPancake", 0) + 1);
        }
    }
}
