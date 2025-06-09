using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChiliDogDialogueCashier : MonoBehaviour
{
    [SerializeField] private GameObject textbox;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private TextMeshProUGUI textNormal, textJittered;
    [SerializeField] private TextMeshProUGUI nameComponent;

    [SerializeField] private float textSpeed;
    [SerializeField] private AudioSource dialogueSFX;

    [SerializeField] private DialogueLine[] dialogueLines;

    private int index;
    private bool isTyping = false;
    private bool isStart = false;

    public AudioSource cashierTheme, walkSound;
    public GameObject witch, evelyn;

    public Animator transiAnim;

    public GameObject skipButton;

    void Start()
    {
        textComponent = textNormal;
        textComponent.text = string.Empty;
        nameComponent.text = string.Empty;

        StartCoroutine(DelayStartDialogue());
    }

    IEnumerator DelayStartDialogue()
    {
        yield return new WaitForSeconds(0.8f);
        walkSound.Play();
        yield return new WaitForSeconds(2.5f);

        cashierTheme.Play();
        witch.SetActive(true);
        textbox.SetActive(true);
        isStart = true;
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
        textbox.SetActive(true);
        DisplayLine();
    }

    public void NextLine()
    {
        if (index == 12)
        {
            textComponent.text = string.Empty;
            textComponent = textJittered;
        }
        else if (index == 13)
        {
            textComponent.text = string.Empty;
            textComponent = textNormal;
        }

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
        skipButton.SetActive(false);
        witch.SetActive(false);
        evelyn.SetActive(false);
        textbox.SetActive(false);

        isStart = false;

        yield return new WaitForSeconds(0.5f);
        transiAnim.SetBool("transi", true);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("ChilidogCooking");
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
            evelyn.SetActive(false);
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

    public void SkipDialogue()
    {
        StartCoroutine(DelayEndScene());
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("isStillInGame", "false");
    }
}
