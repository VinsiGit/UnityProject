using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managµment : MonoBehaviour
{
    public int playerHealth = 100; // Player's health
    public int playerScore = 0; // Player's score

    // Start is called before the first frame update
    void Start()
    {
        // Print the initial player health and score
        Debug.Log("Player Health: " + playerHealth);
        Debug.Log("Player Score: " + playerScore);
    }

    // Method to decrease player health
    public void DecreaseHealth(int amount)
    {
        playerHealth -= amount;
        Debug.Log("Player Health: " + playerHealth);
    }

    // Method to increase player score
    public void IncreaseScore(int amount)
    {
        playerScore += amount;
        Debug.Log("Player Score: " + playerScore);
    }
}