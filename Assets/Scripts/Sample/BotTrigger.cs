using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotTrigger : MonoBehaviour
{
    public DragDropContrrol dragDropControl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopWhisk()
    {
        GetComponent<Animator>().speed = 1;
        dragDropControl.isOver = true;
    }
}
