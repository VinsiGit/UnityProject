using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToLevel3 : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerManager playerManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.score == 7)
        {
            //Dit achteraf veranderen naar level3
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
