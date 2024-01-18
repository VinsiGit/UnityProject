using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI interactionText; // Reference to TextMeshProUGUI component
    public TextMeshProUGUI timerDisplay;
    public TextMeshProUGUI progressDisplay;
    public TextMeshProUGUI goalDisplay;
    public GameObject dialogueWindow;

    private TextMeshProUGUI output_text; // New TextMeshProUGUI for dialogue
    private TextMeshProUGUI next; // New TextMeshProUGUI for dialogue
    private float dialogueTypingDelay = 0.1f; // Typing speed for interaction text
    private Coroutine dialogueCoroutine; // Coroutine reference for interaction text

    // Start is called before the first frame update
    void Start()
    {
        // Initialize interactionTextCoroutine
        dialogueCoroutine = null;
        output_text = dialogueWindow.transform.Find("text").GetComponent<TextMeshProUGUI>();
        next = dialogueWindow.transform.Find("next").GetComponent<TextMeshProUGUI>();
        string[] dialogueStrings = new string[]
        {
                "Hello, welcome to the dialogue!",
                "This is the second line.",
                "And here is the third line.",
                "Press space to continue..."
        };
        TypeDialogue(dialogueStrings);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Public method to call interaction text from the outside

    // Public method to call interaction text from the outside
    public void TypeDialogue(string[] texts)
    {
        next.gameObject.SetActive(false);
        if (dialogueCoroutine != null)
        {
            // Stop the existing coroutine if it's running
            StopCoroutine(dialogueCoroutine);
        }

        // Use coroutine for interaction text with typewriter effect
        dialogueCoroutine = StartCoroutine(TypeDialogueCoroutine(texts));

        // Activate the interaction text
        dialogueWindow.gameObject.SetActive(true);
    }

    private IEnumerator TypeDialogueCoroutine(string[] texts)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            output_text.text = ""; // Ensure the text is initially empty

            int characterIndex = 0;

            while (characterIndex < texts[i].Length)
            {
                // Append one character to the dialogue window
                output_text.text += texts[i][characterIndex];
                characterIndex++;

                // Wait for the specified time before typing the next character
                yield return new WaitForSeconds(dialogueTypingDelay);
            }

            next.gameObject.SetActive(true);

            // Wait until the spacebar is pressed
            while (!Input.GetKey(KeyCode.Space))
            {
                yield return null;
            }

            // Clear the dialogue window after the spacebar is pressed
            output_text.text = "";
            next.gameObject.SetActive(false);
        }

        // Deactivate the dialogue window after the last string
        dialogueWindow.gameObject.SetActive(false);
    }

    public void InteractionTextActive(bool active, string text = "")
    {
        if (text != "")
        {
            interactionText.text = $"{text}: [e]";
        }
        else
        {
            interactionText.text = $"[e]";
        }

        interactionText.gameObject.SetActive(active);
    }

    public void UpdateTimer(string text, string value)
    {
        timerDisplay.text = $"{text}: {value}";
    }

    public void TimerDisplayActive(bool active)
    {
        timerDisplay.gameObject.SetActive(active);
    }

    public void DisplayQuestInfo(string goal, string initialProgressState)
    {
        goalDisplay.text = $"goal: {goal}";
        progressDisplay.text = initialProgressState;
        goalDisplay.gameObject.SetActive(true);
        progressDisplay.gameObject.SetActive(true);
    }

    public void UpdateQuestProgress(string progress)
    {
        progressDisplay.text = progress;
    }

    public void HideQuestInfo()
    {
        goalDisplay.gameObject.SetActive(false);
        progressDisplay.gameObject.SetActive(false);
    }
}
