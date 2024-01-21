using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public TextMeshProUGUI scoreDisplay; // direct reference to textmesh -> change to inventory attribute of playermanager
    public TextMeshProUGUI healthDisplay; // display for player's health

    private static int score = 0;
    private int health = 10; // player's health

    public static int Score
    {
        get { return score; }
    }
    public int Health
    {
        get { return health; }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplay.text = $"Score: {score}";
        healthDisplay.text = $"Health: {health}"; // display player's health

    }

    public void addScore(int amount)
    {
        score += amount;
    }
    public void Damage(int amount)
    {
        health -= amount; // decrease player's health by the damage amount
        if (health < 0) health = 0; // prevent health from going below 0
    }

}
