using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CookIntro : MonoBehaviour
{
    [SerializeField] private GameObject textbox;
    [SerializeField] private GameObject textboxTutor;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private TextMeshProUGUI textTutComponent;
    [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed;
    [SerializeField] private AudioSource dialogueSFX;

    public int index;
    public Image witchImg;
    public Sprite smileSpr;

    private bool check;

    bool isTutorial = false;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1.5f);
        check = true;
        witchImg.gameObject.SetActive(true);
        textbox.SetActive(true);
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (check && !isTutorial)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                if (textComponent.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
            }
        }
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
        if (index == 0)
        {
            witchImg.sprite = smileSpr;
        }
        else if (index == 2)
        {
            witchImg.gameObject.SetActive(false);
            textbox.SetActive(false);
            textboxTutor.SetActive(true);
            textSpeed = 0.01f;

            textComponent = textTutComponent;

            isTutorial = true;
        }

        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            StartCoroutine(DialogueSFX());
        }
        else
        {
            check = false;
            gameObject.SetActive(false);
        }
    }
}
