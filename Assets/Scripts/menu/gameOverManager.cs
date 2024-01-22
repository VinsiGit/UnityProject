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
        AudioListener.volume = 1f;
        initialScore = PlayerManager.Score;
    }

    void Update()
    {
    }

    public void RestartGame()
    {
        PlayerManager.Score = initialScore;
        stateMan.ResumeGame();
        AudioListener.volume = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenu()
    {
        stateMan.ResumeGame();
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        AudioListener.volume = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
