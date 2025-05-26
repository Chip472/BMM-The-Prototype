using UnityEngine;
using TMPro;
using System.Collections;

public class PancakeManager : MonoBehaviour
{
    public TMP_Text scaleNumber;
    public IngredientPourReceiver flourReceiver, milkReceiver;
    public Animator ingredientAnim;
    public Animator bowlAnim;

    public GameObject triggerFlour, triggerMilk;
    public GameObject butter, egg;

    public CookIntro cookIntro;

    bool isFlour = true;
    bool check = false;
    bool isDoneDia = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isFlour = true;
        ingredientAnim.speed = 0;
        bowlAnim.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDoneDia && cookIntro.index == 3)
        {
            isDoneDia = true;
            ingredientAnim.speed = 1;
            bowlAnim.speed = 1;
        }

        if (isFlour)
            scaleNumber.text = flourReceiver.currentWeight + "g";
        else
            scaleNumber.text = milkReceiver.currentWeight + "g";

        if (butter.activeSelf && egg.activeSelf)
        {
            if (!check)
            {
                check = true;
                FromEggButterToMilk();
            }
        }
    }

    public void FromFlourToEggButter()
    {
        if (cookIntro != null)
        {
            cookIntro.NextLine();
        }

        triggerFlour.SetActive(false);
        ingredientAnim.SetBool("step2", true);
    }

    public void FromEggButterToMilk()
    {
        if (cookIntro != null)
        {
            cookIntro.NextLine();
        }

        isFlour = false;
        triggerMilk.SetActive(true);
        ingredientAnim.SetBool("step3", true);
    }

    public void FromMilkToWhisk()
    {
        if (cookIntro != null)
        {
            cookIntro.NextLine();
        }

        ingredientAnim.SetBool("step4", true);
    }
}
