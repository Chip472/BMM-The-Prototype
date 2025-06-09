using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Video;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Stuff")]
    public GameObject book;
    public GameObject bookBG;
    public GameObject settingsPage;
    public Animator transiAnim;
    public Animator transiAnimInGame;

    [Header("Audio Settings")] 
    public AudioMixer audioMixer;
    public Slider musicSlider, sfxSlider;

    [Header("Resolution Settings")]
    public Vector2[] linePos;
    public Transform line;

    [Header("Quality Settings")]
    public Vector2[] plusPos;
    public Transform plus;

    [Header("Prologue and different stuff")]
    public GameObject prologueCanvas;
    public VideoPlayer prologuePlayer;
    public GameObject skipButton;
    public GameObject blackScreen;

    public GameObject chapter1Sticku, chapter2Sticku;
    public GameObject letterChap1, letterChap2;
    public GameObject goodEndTextChap1, badEndTextChap1;
    public GameObject goodEndTextChap2, badEndTextChap2;

    [Header("Sounds")]
    public AudioSource bookSFX;
    public AudioSource letterSFX;
    public AudioSource menuTheme;

    private void Awake()
    {
        if (PlayerPrefs.GetString("isStillInGame", "false") == "true")
        {
            transiAnim = transiAnimInGame;
            transiAnimInGame.gameObject.SetActive(true);
        }
        else
        {
            prologuePlayer.loopPointReached += OnVideoEnd;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 0f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 0f);

        musicSlider.value = musicVol;
        sfxSlider.value = sfxVol;

        SetMusicVolume(musicVol);
        SetSFXVolume(sfxVol);

        LoadResolution();
        LoadQuality();

        if (PlayerPrefs.GetString("isStillInGame", "false") == "true")
        {
            menuTheme.Play();

            skipButton.SetActive(false);
            blackScreen.SetActive(false);
            prologueCanvas.SetActive(false);
            prologuePlayer.gameObject.SetActive(false);

            if (PlayerPrefs.GetString("isChapter1End", "none") == "happy")
            {
                if (PlayerPrefs.GetString("isHappyEnd1Unlocked", "false") == "false" && PlayerPrefs.GetString("isChapter1Done", "false") == "true")
                {
                    PlayerPrefs.SetString("isHappyEnd1Unlocked", "true");
                    PlayButton();
                }
            }
            else if (PlayerPrefs.GetString("isChapter1End", "none") == "bad")
            {
                if (PlayerPrefs.GetString("isBadEnd1Unlocked", "false") == "false" && PlayerPrefs.GetString("isChapter1Done", "false") == "true")
                {
                    PlayerPrefs.SetString("isBadEnd1Unlocked", "true");
                    PlayButton();
                }
            }

            if (PlayerPrefs.GetString("isChapter2End", "none") == "happy")
            {
                if (PlayerPrefs.GetString("isHappyEnd2Unlocked", "false") == "false" && PlayerPrefs.GetString("isChapter2Done", "false") == "true")
                {
                    PlayerPrefs.SetString("isHappyEnd2Unlocked", "true");
                    PlayButton();
                }
            }
            else if (PlayerPrefs.GetString("isChapter2End", "none") == "bad")
            {
                if (PlayerPrefs.GetString("isBadEnd2Unlocked", "false") == "false" && PlayerPrefs.GetString("isChapter2Done", "false") == "true")
                {
                    PlayerPrefs.SetString("isBadEnd2Unlocked", "true");
                    PlayButton();
                }
            }
        }

        if (PlayerPrefs.GetString("isChapter1Done", "false") == "true")
        {
            chapter1Sticku.SetActive(true);
            if (PlayerPrefs.GetString("sticker1Clicked", "false") == "false")
            {
                chapter1Sticku.GetComponent<Animator>().SetBool("blink", true);
            }
        }

        if (PlayerPrefs.GetString("isChapter2Done", "false") == "true")
        {
            chapter2Sticku.SetActive(true);
            if (PlayerPrefs.GetString("sticker2Clicked", "false") == "false")
            {
                chapter2Sticku.GetComponent<Animator>().SetBool("blink", true);
            }
        }
    }
    void OnVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(DelaySkipPrologue());
    }
    public void SkipPrologue()
    {
        StartCoroutine(DelaySkipPrologue());
    }

    IEnumerator DelaySkipPrologue()
    {
        prologueCanvas.SetActive(false);
        prologuePlayer.gameObject.SetActive(false);
        skipButton.SetActive(false);

        yield return new WaitForSeconds(1f);

        transiAnim.gameObject.SetActive(true);
        blackScreen.SetActive(false);
        menuTheme.Play();
    }

    // ----- MENU & BOOK -----

    public void PlayButton()
    {
        bookSFX.Play();

        book.SetActive(true);
        bookBG.SetActive(true);
        settingsPage.SetActive(false);
    }

    public void ExitButton()
    {
        PlayerPrefs.SetString("isStillInGame", "false");
        Application.Quit();
    }

    public void OpenChapterPage()
    {
        letterSFX.Play();
        settingsPage.SetActive(false);
    }

    public void OpenSettingsPage()
    {
        letterSFX.Play();
        settingsPage.SetActive(true);
    }

    public void CloseBook()
    {
        book.SetActive(false);
        bookBG.SetActive(false);
    }

    public void OpenLetterChapter1()
    {
        letterSFX.Play();

        letterChap1.SetActive(true);
        if (PlayerPrefs.GetString("isChapter1End", "happy") == "happy"){
            goodEndTextChap1.SetActive(true);
        }
        else
        {
            badEndTextChap1.SetActive(true);
        }
        PlayerPrefs.SetString("sticker1Clicked", "true");
        chapter1Sticku.GetComponent<Animator>().SetBool("blink", false);
        chapter1Sticku.GetComponent<Animator>().Play("MenuStickerStay", 0);
    }

    public void CloseLetterChapter1()
    {
        letterChap1.SetActive(false);
    }

    public void OpenLetterChapter2()
    {
        letterSFX.Play();

        letterChap2.SetActive(true);
        if (PlayerPrefs.GetString("isChapter2End", "happy") == "happy")
        {
            goodEndTextChap2.SetActive(true);
        }
        else
        {
            badEndTextChap2.SetActive(true);
        }
        PlayerPrefs.SetString("sticker2Clicked", "true");
        chapter2Sticku.GetComponent<Animator>().SetBool("blink", false);
        chapter2Sticku.GetComponent<Animator>().Play("MenuStickerStay", 0);
    }

    public void CloseLetterChapter2()
    {
        letterChap2.SetActive(false);
    }


    // ----- CHAPTER CHOOSING -----

    public void ChapterOne()
    {
        StartCoroutine(DelayChangeScene("Chapter1Cashier"));
    }

    public void ChapterTwo()
    {
        StartCoroutine(DelayChangeScene("Chapter2Cashier"));
    }

    IEnumerator DelayChangeScene(string sceneName)
    {
        transiAnim.SetBool("transi", true);
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(sceneName);
    }

    // ----- VOLUME -----

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }


    // ----- RESOLUTION -----

    public void SetResolution1280()
    {
        SetResolution(1280, 720, 0);
    }

    public void SetResolution1920()
    {
        SetResolution(1920, 1080, 1); // Default
    }

    public void SetResolution3840()
    {
        SetResolution(3840, 2160, 2);
    }

    private void SetResolution(int width, int height, int index)
    {
        Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
        PlayerPrefs.SetInt("Resolution", index);
        ResetLineSprite(index);
    }

    private void LoadResolution()
    {
        int savedIndex = PlayerPrefs.GetInt("Resolution", 1); // Default to 1920x1080
        switch (savedIndex)
        {
            case 0: SetResolution1280(); break;
            case 2: SetResolution3840(); break;
            default: SetResolution1920(); break;
        }
        ResetLineSprite(savedIndex);
    }

    void ResetLineSprite(int index)
    {
        line.localPosition = linePos[index];
    }

    // ----- QUALITY -----

    public void SetLowQuality()
    {
        SetQualityLevel(0);
    }

    public void SetMediumQuality()
    {
        SetQualityLevel(2);
    }

    public void SetHighQuality()
    {
        SetQualityLevel(5); // Default Unity "High"
    }

    private void SetQualityLevel(int level)
    {
        QualitySettings.SetQualityLevel(level);
        PlayerPrefs.SetInt("Quality", level);

        ResetPlusSprite(level);
    }

    private void LoadQuality()
    {
        int savedQuality = PlayerPrefs.GetInt("Quality", 5); // Default to High
        QualitySettings.SetQualityLevel(savedQuality);

        ResetPlusSprite(savedQuality);
    }

    void ResetPlusSprite(int index)
    {
        switch (index)
        {
            case 0:
                plus.localPosition = plusPos[2];
                break;
            case 2:
                plus.localPosition = plusPos[1];
                break;
            case 5:
                plus.localPosition = plusPos[0];
                break;
        }
    }
}
