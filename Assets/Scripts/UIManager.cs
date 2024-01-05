using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI interactionText; // Reference to TextMeshProUGUI component
    public TextMeshProUGUI timerDisplay;

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
}
