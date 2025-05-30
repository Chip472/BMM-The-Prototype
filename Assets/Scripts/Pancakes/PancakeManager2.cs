using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PancakeManager2 : MonoBehaviour
{
    public GameObject chaobep, uiNuongBanh;
    public GameObject doTrangTri, finalDish;

    public Animator decorAnim;
    public Animator pancakeAnim, tableAnim, starBGAnim;

    bool check = false;
    bool check2 = false;
    bool check3 = false;

    public GameObject syrup, bluebweey;

    public GameObject drawingPad;
    public Texture2D wandCursor;
    public GameObject kirakira;

    public Animator transition;

    public CookIntro2 cookIntro;

    public AudioSource sizzleSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!check && PlayerPrefs.GetInt("cookedPancake", 0) == 3)
        {
            check = true;
            sizzleSFX.Stop();
            chaobep.GetComponent<Animator>().speed = 0;
            chaobep.GetComponent<PancakeCooker>().enabled = false;
            StartCoroutine(DelayChangeScene());
        }

        if (!check2 && syrup.activeSelf)
        {
            check2 = true;
            decorAnim.SetBool("step3", true);
        }
        
        if (!check3 && bluebweey.activeSelf)
        {
            check3 = true;
            starBGAnim.gameObject.SetActive(true);

            decorAnim.SetBool("final", true);
            pancakeAnim.SetBool("final", true);
            tableAnim.SetBool("final", true);

            StartCoroutine(DelaySceneChant());
        }
    }

    IEnumerator DelaySceneChant()
    {
        cookIntro.NextLine();
        yield return new WaitForSeconds(3f);

        drawingPad.SetActive(true);
        gameObject.GetComponent<ChantingScript>().enabled = true;
        Cursor.SetCursor(wandCursor, Vector2.zero, CursorMode.Auto);
        kirakira.SetActive(true) ;
    }

    IEnumerator DelayChangeScene()
    {
        yield return new WaitForSeconds(1f);

        PlayerPrefs.SetInt("cookedPancake", 0);

        chaobep.SetActive(false);
        uiNuongBanh.SetActive(false);

        cookIntro.textbox.SetActive(true);
        cookIntro.NextLine();

        doTrangTri.SetActive(true);
        finalDish.SetActive(true);
    }

    public void ChangeToStep2()
    {
        decorAnim.SetBool("step2", true);
        cookIntro.NextLine();
    }

    public void ChangeToDream()
    {
        Debug.Log(PlayerPrefs.GetFloat("chap1Score"));
        StartCoroutine(DelayTransi());
    }

    IEnumerator DelayTransi()
    {
        transition.gameObject.SetActive(true);
        transition.Play("TransitionIn", 0);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Chapter1Dream");
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("isStillInGame", "false");
    }
}
