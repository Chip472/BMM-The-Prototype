using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PancakeDreamManager : MonoBehaviour
{
    public Button skipButton;
    public GameObject dreamScene, cashierScene;
    public GameObject goodDialogue, badDialogue;

    public AudioSource windAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (PlayerPrefs.GetFloat("chap1Score", 0) >= 4.0f)
        {
            PlayerPrefs.SetString("isChapter1End", "happy");
            GetComponent<PancakeGoodDream>().enabled = true;
            skipButton.onClick.AddListener(GetComponent<PancakeGoodDream>().SkipDialogue);
        }
        else
        {
            PlayerPrefs.SetString("isChapter1End", "bad");
            GetComponent<PancakeBadDream>().enabled = true;
            skipButton.onClick.AddListener(GetComponent<PancakeBadDream>().SkipDialogue);
        }
    }

    public void ChangeToCashier()
    {
        cashierScene.SetActive(true);
        dreamScene.SetActive(false);

        windAudio.Stop();
        GetComponent<PancakeGoodDream>().enabled = false;
        GetComponent<PancakeBadDream>().enabled = false;

        if (PlayerPrefs.GetFloat("chap1Score", 0) >= 4.0f)
        {
            goodDialogue.SetActive(true);
        }
        else
        {
            badDialogue.SetActive(true);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("isStillInGame", "false");
    }
}
