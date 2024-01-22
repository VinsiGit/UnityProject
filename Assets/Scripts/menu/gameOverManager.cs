using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOverManager : MonoBehaviour
{
    public StateManager stateMan;

    private int initialScore = 0;

    void Start()
    {
        Debug.Log("game over initialised");
        initialScore = PlayerManager.Score;
    }

    void Update()
    {
    }

    public void RestartGame()
    {
        AudioListener.volume = 1f;
        PlayerManager.Score = initialScore;
        stateMan.ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenu()
    {
        AudioListener.volume = 1f;
        stateMan.ResumeGame();
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
