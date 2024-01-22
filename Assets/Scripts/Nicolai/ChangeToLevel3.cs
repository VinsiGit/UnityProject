using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToLevel3 : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerManager playerManager;
    private int initialscore = 0;
    void Start()
    {
        initialscore = PlayerManager.Score;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.score-initialscore == 3)
        {
            //Dit achteraf veranderen naar level3
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
