using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingBar : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider progressBar;

    public float fakeLoadDuration = 2f;
    public float minLoadingTime = 2f;

    public AudioSource menuTheme;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadWithMinimumTime(sceneName));
    }

    IEnumerator LoadWithMinimumTime(string sceneName)
    {
        loadingScreen.SetActive(true);
        progressBar.value = 0;
        Debug.Log("0%");

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float fakeTimer = 0f;

        while (!operation.isDone)
        {
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);
            fakeTimer += Time.deltaTime;

            // Blend real progress with time-based progress
            float blendedProgress = Mathf.Min(realProgress, fakeTimer / minLoadingTime);

            progressBar.value = blendedProgress;
            Debug.Log((blendedProgress * 100f).ToString("F0") + "%");

            // Only allow activation if:
            // 1. Scene is fully loaded (realProgress >= 0.9)
            // 2. AND minimum fake time has passed
            if (realProgress >= 1f && fakeTimer >= minLoadingTime)
            {
                Debug.Log("Loading Complete!");
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void LoadFakeScene(GameObject sceneToShow)
    {
        StartCoroutine(FakeLoadRoutine(sceneToShow));
    }

    IEnumerator FakeLoadRoutine(GameObject sceneToShow)
    {
        loadingScreen.SetActive(true);

        float timer = 0f;

        while (timer < fakeLoadDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / fakeLoadDuration);
            progressBar.value = progress;
            Debug.Log((progress * 100f).ToString("F0") + "%");

            yield return null;
        }

        Debug.Log("Fake scene loaded!");
        sceneToShow.SetActive(true);
        menuTheme.Play();

        loadingScreen.SetActive(false);
    }
}
