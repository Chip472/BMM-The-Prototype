using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChiliDogPostDialogue : MonoBehaviour
{
    [SerializeField] private GameObject textbox;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private TextMeshProUGUI nameComponent;

    [SerializeField] private float textSpeed;
    [SerializeField] private AudioSource dialogueSFX;

    [SerializeField] private DialogueLine[] dialogueLines;

    public Animator transiAnim;

    private int index;
    private bool isTyping = false;
    private bool isStart = false;

    public GameObject witch, bob;

    void Start()
    {
        textComponent.text = string.Empty;
        nameComponent.text = string.Empty;

        StartCoroutine(DelayStartDialogue());
    }

    IEnumerator DelayStartDialogue()
    {
        yield return new WaitForSeconds(1.5f);
        StartDialogue();
    }

    void Update()
    {
        if (isStart && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                textComponent.maxVisibleCharacters = dialogueLines[index].lineText.Length;
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialogue()
    {
        index = 0;
        bob.SetActive(true);
        textbox.SetActive(true);
        isStart = true;
        DisplayLine();
    }

    public void NextLine()
    {

        if (index < dialogueLines.Length - 1)
        {
            index++;
            DisplayLine();
        }
        else
        {
            StartCoroutine(DelayEndScene());
        }
    }

    IEnumerator DelayEndScene()
    {
        transiAnim.SetBool("transi", true);
        PlayerPrefs.SetString("isStillInGame", "true");
        PlayerPrefs.SetString("isChapter2Done", "true");

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MenuScene");
    }

    void DisplayLine()
    {
        DialogueLine line = dialogueLines[index];

        //speaker's name
        if (line.speakerName != null)
            nameComponent.text = line.speakerName;
        else
            nameComponent.text = "";

        //character sprite
        if (line.characterImg != null)
        {
            if (line.characterSprite != null)
                line.characterImg.sprite = line.characterSprite;
        }
        else
        {
            witch.SetActive(false);
            bob.SetActive(false);
        }

        //speaker GameObject visibility
        if (line.characterObject != null)
            line.characterObject.SetActive(true);

        if (line.othersToHide != null)
            line.othersToHide.SetActive(false);


        textComponent.text = string.Empty;

        if (line.lineText != null)
        {
            StartCoroutine(TypeLine(line.lineText));
            StartCoroutine(PlayDialogueSFX(line.lineText));
        }
        else
        {
            isTyping = false;
        }
    }

    IEnumerator TypeLine(string lineText)
    {
        isTyping = true;
        textComponent.text = lineText;  // Set full text immediately
        textComponent.maxVisibleCharacters = 0;

        for (int i = 0; i <= lineText.Length; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    IEnumerator PlayDialogueSFX(string lineText)
    {
        foreach (char c in lineText.ToCharArray())
        {
            if (!isTyping) yield break;
            dialogueSFX.Play();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
