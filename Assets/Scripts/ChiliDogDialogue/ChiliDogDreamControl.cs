using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChiliDogDreamControl : MonoBehaviour
{
    public Button skipButton;
    public GameObject cashierScene, dreamScene;
    public GameObject goodEndScene, badEndScene;

    private void Awake()
    {
        if (PlayerPrefs.GetFloat("chap2Score", 0) >= 5f)
        {
            PlayerPrefs.SetString("isChapter2End", "happy");
            GetComponent<ChiliDogGoodDream>().enabled = true;
            skipButton.onClick.AddListener(GetComponent<ChiliDogGoodDream>().SkipDialogue);
        }
        else
        {
            PlayerPrefs.SetString("isChapter2End", "bad");
            GetComponent<ChiliDogBadDream>().enabled = true;
            skipButton.onClick.AddListener(GetComponent<ChiliDogBadDream>().SkipDialogue);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeToCashier()
    {
        cashierScene.SetActive(true);
        dreamScene.SetActive(false);

        GetComponent<ChiliDogGoodDream>().enabled = false;
        GetComponent<ChiliDogBadDream>().enabled = false;

        if (PlayerPrefs.GetFloat("chap2Score", 0) >= 5f)
        {
            goodEndScene.SetActive(true);
        }
        else
        {
            badEndScene.SetActive(true);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("isStillInGame", "false");
    }
}
