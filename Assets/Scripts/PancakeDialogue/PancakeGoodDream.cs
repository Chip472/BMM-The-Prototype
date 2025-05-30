using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PancakeGoodDream : MonoBehaviour
{
    [SerializeField] private GameObject textbox;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private TextMeshProUGUI nameComponent;

    [SerializeField] private float textSpeed;
    [SerializeField] private AudioSource dialogueSFX;

    [SerializeField] private DialogueLine[] dialogueLines;

    private int index;
    private bool isTyping = false;
    private bool isStart = false;

    public GameObject son, bob;

    public Animator transiAnim;
    public AudioSource trainHorn, trainArrive, trainGo, walkSound;

    public GameObject trainBG, skipButton;

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

    bool check = false;

    public void NextLine()
    {
        if (index == 0 && !check)
        {
            check = true;
            isStart = false;
            StartCoroutine(DelayTrainSound());
            return;
        }
        else if (index == 3)
        {
            walkSound.Play();
        }
        else if (index == 8)
        {
            trainGo.Play();
            trainBG.GetComponent<Animator>().SetBool("traingo", true);
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

    IEnumerator DelayTrainSound()
    {
        textbox.SetActive(false);
        bob.SetActive(false);
        trainHorn.Play();

        yield return new WaitForSeconds(1f);
        trainArrive.Play();

        yield return new WaitForSeconds(1.5f);
        trainBG.SetActive(true);

        yield return new WaitForSeconds(2.5f);
        isStart = true;

        textbox.SetActive(true);
        bob.SetActive(true);
        NextLine();
    }

    IEnumerator DelayEndScene()
    {
        skipButton.SetActive(false);
        transiAnim.SetBool("transi", true);

        yield return new WaitForSeconds(1.5f);
        GetComponent<PancakeDreamManager>().ChangeToCashier();
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
            son.SetActive(false);
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

    public void SkipDialogue()
    {
        isStart = false;
        StartCoroutine(DelayEndScene());
    }
}
