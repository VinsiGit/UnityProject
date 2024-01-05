using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class time : MonoBehaviour
{
    public gameTimer timescript;

    // Start is called before the first frame update
    void Start()
    {
        timescript.StartTimer(1000, "radiation level ");
    }

    // Update is called once per frame
    void Update()
    {
        if (timescript.timerComplete == true)
        {
            Debug.Log("Dood"); // veranderen naar echt dood gaan
            timescript.timerComplete = false;
        }
    }
}
