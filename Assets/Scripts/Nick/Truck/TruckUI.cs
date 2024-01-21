using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TruckUI : MonoBehaviour
{
    public GameObject dialogueWindow;

    private TextMeshProUGUI output_text; // New TextMeshProUGUI for dialogue
    private TextMeshProUGUI next; // New TextMeshProUGUI for dialogue
    private float dialogueTypingDelay = 0.005f; // Typing speed for interaction text
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
                "QUICK! FIND THE GARBAGE DISPOSAL FACTORY BEFORE YOU LOSE EVERYTHING!!!"
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
}
