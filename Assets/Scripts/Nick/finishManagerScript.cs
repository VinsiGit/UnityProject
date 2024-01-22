using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishManagerScript : MonoBehaviour
{

    public GameObject finishUI;
    private int initialScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        initialScore = PlayerManager.Score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Finished()
    {
        Cursor.lockState = CursorLockMode.None;
        finishUI.SetActive(true);
    }

    public void Restart()
    {
        PlayerManager.Score = initialScore;
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
        SceneManager.LoadScene("starting_menu");
    }

    public void quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
