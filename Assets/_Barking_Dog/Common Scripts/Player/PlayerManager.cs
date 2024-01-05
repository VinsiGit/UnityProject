using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public TextMeshProUGUI scoreDisplay; // direct reference to textmesh -> change to inventory attribute of playermanager

    public int score = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplay.text = $"Score: {score}";
    }

    public void addScore(int amount)
    {
        score += amount;
    }
}
