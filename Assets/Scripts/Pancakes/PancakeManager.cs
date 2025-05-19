using UnityEngine;
using TMPro;

public class PancakeManager : MonoBehaviour
{
    public TMP_Text scaleNumber;
    public IngredientPourReceiver flourReceiver, milkReceiver;
    public Animator ingredientAnim;

    public GameObject triggerFlour, triggerMilk;
    public GameObject butter, egg;

    bool isFlour = true;
    bool check = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isFlour = true;
    }

    // Update is called once per frame
    void Update()
    {
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
        triggerFlour.SetActive(false);
        ingredientAnim.SetBool("step2", true);
    }

    public void FromEggButterToMilk()
    {
        isFlour = false;
        triggerMilk.SetActive(true);
        ingredientAnim.SetBool("step3", true);
    }

    public void FromMilkToWhisk()
    {
        ingredientAnim.SetBool("step4", true);
    }
}
