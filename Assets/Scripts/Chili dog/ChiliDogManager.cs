using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChiliDogManager : MonoBehaviour
{
    public GameObject rawMeatInPan;
    public Animator fryingAnim;

    public GameObject sliderObj;

    public Animator decorAnim;

    bool checkStep1 = false;

    public GameObject ketchup, mustard;
    bool checkStep2 = false;

    public GameObject rauMui;
    public SpriteRenderer cheese;
    bool checkStep3 = false;

    public Animator tableAnim;
    public GameObject skyObj, drawingPad;
    public Texture2D wandCursor;
    public GameObject kirakira;

    public Animator transiAnim;
    public ChiliDogTutor tutorText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetFloat("chap2Score", 0);
        StartCoroutine(DelayStartScene());
    }

    IEnumerator DelayStartScene()
    {
        yield return new WaitForSeconds(1.5f);
        tutorText.gameObject.SetActive(true);
        tutorText.textbox.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!checkStep1 && rawMeatInPan.activeSelf)
        {
            checkStep1 = true;

            fryingAnim.SetBool("step4", true);
            sliderObj.SetActive(true);
            tutorText.NextLine();
        }

        if (!checkStep2 && ketchup.activeSelf && mustard.activeSelf)
        {
            checkStep2 = true;
            decorAnim.SetBool("Step3", true);
        }

        if (!checkStep3 &&  rauMui.activeSelf && cheese.color.a == 1f)
        {
            checkStep3 = true;
            decorAnim.SetBool("Final", true);
            tableAnim.SetBool("final", true);
            skyObj.SetActive(true);
            StartCoroutine(DelaySceneChant());
        }
    }

    IEnumerator DelaySceneChant()
    {
        tutorText.NextLine();
        yield return new WaitForSeconds(3f);

        drawingPad.SetActive(true);
        gameObject.GetComponent<ChantingScript>().enabled = true;
        Cursor.SetCursor(wandCursor, Vector2.zero, CursorMode.Auto);
        kirakira.SetActive(true);
    }

    public void ChangeToStep2()
    {
        decorAnim.SetBool("Step2", true);
        tutorText.NextLine();
    }
    public void ChangeScene()
    {
        StartCoroutine(DelayEndScene());
    }

    IEnumerator DelayEndScene()
    {
        transiAnim.SetBool("transi", true);

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Chapter2Dream");
    }
}
