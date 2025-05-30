using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookIntro2 : MonoBehaviour
{
    public GameObject textbox;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed;
    [SerializeField] private AudioSource dialogueSFX;

    public int index;
    bool check = false;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (!check && textComponent.text == lines[index])
        {
            check = true;
            StartCoroutine(WaitABit());
        }
    }

    IEnumerator WaitABit()
    {
        yield return new WaitForSeconds(2f);
        textbox.SetActive(false);
    }

    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
        StartCoroutine(DialogueSFX());
    }

    IEnumerator TypeLine()
    {
        string line = lines[index];
        textComponent.text = "";

        int i = 0;
        while (i < line.Length)
        {
            // Check if we're at a tag
            if (line[i] == '<')
            {
                // Read the full tag
                int tagCloseIndex = line.IndexOf('>', i);
                if (tagCloseIndex != -1)
                {
                    string tag = line.Substring(i, tagCloseIndex - i + 1);
                    textComponent.text += tag;
                    i = tagCloseIndex + 1;
                    continue;
                }
            }

            // Add one visible character
            textComponent.text += line[i];
            i++;
            yield return new WaitForSeconds(textSpeed);
        }
    }


    IEnumerator DialogueSFX()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            if (textComponent.text != lines[index])
            {
                dialogueSFX.Play();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void NextLine()
    {

        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            StartCoroutine(DialogueSFX());
        }
        else
        {
            textbox.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
