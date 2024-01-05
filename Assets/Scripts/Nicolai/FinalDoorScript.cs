using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FinalDoorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform doorTransform;  // Reference to the transform of the door object
    public PlayerManager playerManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (playerManager.score == 10)
        {
            Destroy(doorTransform.gameObject);
        }
    }
}
