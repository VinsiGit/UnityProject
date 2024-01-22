using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Voeg deze regel toe om toegang te krijgen tot SceneManager

public class time : MonoBehaviour
{
    public GameTimer timescript;
    public int timer = 500;
    public StateManager state;




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
            Debug.Log("Dood"); // Veranderd naar echt dood gaan
            state.GameOver();
            timescript.timerComplete = false;
        }
    }


}