using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; //thư viện cho các thứ liên quan đến chuyển scene

public class BotTrigger : MonoBehaviour
{
    public DragDropContrrol dragDrop;

    public Animator transiAnim;

    public void StopWhisk()
    {
        dragDrop.isPhraseOver = true;
        GetComponent<Animator>().speed = 1;
        GetComponent<Animator>().SetBool("isThisSceneDone", true);
    }

    public void ChangeToNextScene()
    {
        transiAnim.SetBool("isSceneDone", true);
        StartCoroutine(DelayChangeScene());
    }

    IEnumerator DelayChangeScene()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("SecondSampleScene");
    }
}
