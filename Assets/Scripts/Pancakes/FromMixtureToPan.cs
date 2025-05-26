using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FromMixtureToPan : MonoBehaviour
{
    public Animator ingredientAnim, bowlAnim;
    public CookIntro cookIntro;

    public GameObject firstP, secondP;
    public GameObject firstC, secondC;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RunAnim()
    {
        StartCoroutine(DelayChangeScene());
    }

    IEnumerator DelayChangeScene()
    {
        if (cookIntro != null)
        {
            cookIntro.NextLine();
        }

        yield return new WaitForSeconds(1f);

        if (ingredientAnim != null && bowlAnim != null)
        {
            ingredientAnim.SetBool("final", true);
            bowlAnim.SetBool("final", true);
        }

        yield return new WaitForSeconds(1.5f);

        secondP.SetActive(true);
        secondC.SetActive(true);
        firstC.SetActive(false);
        firstP.SetActive(false);
    }
}
