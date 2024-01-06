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
    public TextMeshProUGUI dialogueWindow; // New TextMeshProUGUI for dialogue

    private float dialogueTypingDelay = 0.1f; // Typing speed for interaction text
    private Coroutine dialogueCoroutine; // Coroutine reference for interaction text

    // Start is called before the first frame update
    void Start()
    {
        // Initialize interactionTextCoroutine
        dialogueCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Public method to call interaction text from the outside
    public void TypeDialogue(string text)
    {
        if (dialogueCoroutine != null)
        {
            // Stop the existing coroutine if it's running
            StopCoroutine(dialogueCoroutine);
        }

        // Use coroutine for interaction text with typewriter effect
        dialogueCoroutine = StartCoroutine(TypeDialogueCoroutine(text));

        // Activate the interaction text
        dialogueWindow.gameObject.SetActive(true);
    }

    private IEnumerator TypeDialogueCoroutine(string textToWrite)
    {
        dialogueWindow.text = ""; // Ensure the text is initially empty

        int characterIndex = 0;

        while (characterIndex < textToWrite.Length)
        {
            // Append one character to the dialogue window
            dialogueWindow.text += textToWrite[characterIndex];
            characterIndex++;

            // Wait for the specified time before typing the next character
            yield return new WaitForSeconds(dialogueTypingDelay);
        }

        // Wait for the specified display time before clearing the text
        yield return new WaitForSeconds(dialogueTypingDelay);

        // Clear the dialogue window after the display time
        dialogueWindow.text = "";

        // Deactivate the dialogue window
        dialogueWindow.gameObject.SetActive(false);
    }

    public void InteractionTextActive(bool active, string text = "")
    {
        if(text != "")
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
