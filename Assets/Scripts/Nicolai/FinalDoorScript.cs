using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FinalDoorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform doorTransform;  // Reference to the transform of the door object
    public Transform Sign;
    private int initialscore = 0;
    void Start()
    {
        initialscore = PlayerManager.Score;
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerManager.Score-initialscore == 6)
        {
            Destroy(doorTransform.gameObject);
            Destroy(Sign.gameObject);
        }
    }
}
