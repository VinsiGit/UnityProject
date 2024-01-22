using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToLevel3 : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerManager playerManager;
    public UIManager UiManager;
    private int initialscore = 0;
    private int barrelAmount = 7;
    void Start()
    {
        initialscore = PlayerManager.Score;
    }

    // Update is called once per frame
    void Update()
    {
        UiManager.UpdateQuestProgress($"{PlayerManager.Score - initialscore}/{barrelAmount}");
        if (PlayerManager.score-initialscore == barrelAmount)
        {
            //Dit achteraf veranderen naar level3
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
