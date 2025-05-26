using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    public Sprite characterSprite; // Can be null
    public Image characterImg;
    public GameObject characterObject; // The GameObject that represents this speaker
    public GameObject othersToHide; // The others to turn off during this speaker's turn
    public string lineText;
}


public class PancakeCashierDia : MonoBehaviour
{
    [SerializeField] private GameObject textbox;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private TextMeshProUGUI textNormal, textJittered;
    [SerializeField] private TextMeshProUGUI nameComponent;

    [SerializeField] private float textSpeed;
    [SerializeField] private AudioSource dialogueSFX;

    [SerializeField] private Sprite defaultSprite;

    [SerializeField] private DialogueLine[] dialogueLines;

    private int index;
    private bool isTyping = false;
    private bool isStart = false;

    public AudioSource cashierTheme, knockSound, openDoorSound, walkSound;
    public Animator bobAnim;
    public GameObject witch, bob;

    public Animator transiAnim;

    public GameObject flashback, skipButton;

    void Start()
    {
        textComponent = textNormal;
        textComponent.text = string.Empty;
        nameComponent.text = string.Empty;

        StartCoroutine(DelayStartDialogue());
    }

    IEnumerator DelayStartDialogue()
    {
        yield return new WaitForSeconds(1.5f);

        knockSound.Play();
        yield return new WaitForSeconds(2f);
        
        openDoorSound.Play();
        yield return new WaitForSeconds(3f);

        witch.SetActive(true);
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
        if (index == 0)
        {
            walkSound.Play();
        }
        else if (index == 1)
        {
            cashierTheme.Play();
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
        bob.SetActive(false);
        textbox.SetActive(false);

        isStart = false;

        yield return new WaitForSeconds(0.5f);
        transiAnim.SetBool("transi", true);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("PancakeCooking");
    }

    void DisplayLine()
    {
        DialogueLine line = dialogueLines[index];

        // Show speaker name
        nameComponent.text = line.speakerName;

        // Show character sprite
        if (line.characterSprite != null)
            line.characterImg.sprite = line.characterSprite;
        else
            line.characterImg.sprite = defaultSprite;

        // Manage speaker GameObject visibility
        if (line.characterObject != null)
            line.characterObject.SetActive(true);
        if (line.othersToHide != null)
        {
            line.othersToHide.SetActive(false);
        }

        if (index == 1)
        {
            bobAnim.Play("BobAppear", 0);
        }
        else if (index == 11)
        {
            flashback.SetActive(true);
        }
        else if (index == 14)
        {
            textComponent.text = string.Empty;
            textComponent = textJittered;
        }
        else if (index == 15)
        {
            flashback.SetActive(false);
            textComponent.text = string.Empty;
            textComponent = textNormal;
            textJittered.gameObject.SetActive(false);
        }

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

    //IEnumerator TypeLine(string lineText)
    //{
    //    isTyping = true;
    //    foreach (char c in lineText.ToCharArray())
    //    {
    //        textComponent.text += c;
    //        yield return new WaitForSeconds(textSpeed);
    //    }
    //    isTyping = false;
    //}
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
}
