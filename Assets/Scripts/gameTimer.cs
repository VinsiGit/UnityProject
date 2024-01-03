using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameTimer : MonoBehaviour
{
    public UIManager UiManager;
    public bool showPercentage = false;

    private int totalTime = 60;
    private float timer = 60f;
    private bool isTimerRunning = true;
    private string tooltext = "test";

    void Update()
    {
        // Check if the timer is running
        if (isTimerRunning)
        {
            // Check if the timer has not reached 0
            if (timer > 0)
            {
                // Decrease the timer by deltaTime each frame
                timer -= Time.deltaTime;

                if (showPercentage == true)
                {
                    // Calculate the percentage remaining
                    float percentage = (int)(Mathf.Clamp01(timer / totalTime) * 100f);

                    // Check if the percentage has changed
                    if (Mathf.Floor(percentage) < Mathf.Floor((timer + Time.deltaTime) / totalTime * 100f))
                    {
                        // Log the remaining time in percentage
                        UiManager.UpdateTimer(tooltext, $"{100-percentage}%");
                    }
                }
                else
                {
                    // Check if the number of seconds has changed
                    if (Mathf.Floor(timer) < Mathf.Floor(timer + Time.deltaTime))
                    {
                        // Log the remaining time in seconds
                        UiManager.UpdateTimer(tooltext, $"{Mathf.Floor(timer)}s");
                    }
                }
            }
            else
            {
                // Stop the timer when it reaches 0
                isTimerRunning = false;
                UiManager.TimerDisplayActive(false);
            }
        }
    }

    // Function to start the timer
    public void StartTimer(int seconds, string text)
    {
        // Reset the timer
        tooltext = text;
        totalTime = seconds;
        timer = seconds;
        isTimerRunning = true;
        UiManager.TimerDisplayActive(true);
    }
}
