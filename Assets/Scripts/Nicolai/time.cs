using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Voeg deze regel toe om toegang te krijgen tot SceneManager

public class time : MonoBehaviour
{
    public GameTimer timescript;
    public int timer = 500;




    // Start is called before the first frame update
    void Start()
    {
        timescript.StartTimer(timer, "radiation level ");


    }
    // Update is called once per frame
    void Update()
    {
        if (timescript.timerComplete == true)
        {
            // Debug.Log("Dood"); // Veranderd naar echt dood gaan

            StartCoroutine(RestartLevelWithDelay(5f)); // Start de coroutine om het level na 5 seconden te herstarten
            timescript.timerComplete = false;
        }
    }

    IEnumerator RestartLevelWithDelay(float delayTime)
    {
        // Vertraag de tijd
        Time.timeScale = 0.1f;
        // Wacht voor de opgegeven vertraging
        yield return new WaitForSecondsRealtime(delayTime);



        // Herstel de tijd naar normaal
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}