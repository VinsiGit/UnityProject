using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FinalDoorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform doorTransform;  // Reference to the transform of the door object
    public Transform Sign;
    public PlayerManager playerManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerManager.score == 6)
        {
            Destroy(doorTransform.gameObject);
            Destroy(Sign.gameObject);
        }
    }
}
