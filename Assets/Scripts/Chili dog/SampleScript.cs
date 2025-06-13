using UnityEngine;

public class SampleScript : MonoBehaviour
{
    public GameObject lalalaowdl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSettings()
    {
        lalalaowdl.SetActive(true);
    }

    public void CloseSettings()
    {
        lalalaowdl.SetActive(false);
    }

    public void NewButtonFunction()
    {
        Debug.Log("Vừa bấm vào nút màu tím");
    }
}
