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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
