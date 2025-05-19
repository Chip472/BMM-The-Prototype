using UnityEngine;
using UnityEngine.SceneManagement;

public class FromMixtureToPan : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
