using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        gameUI.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCouroutine());
    }

    private IEnumerator GameOverCouroutine()
    {
        // Slow timescale
        Time.timeScale = 0.1f;
        Cursor.lockState = CursorLockMode.None;
        gameUI.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(true);

        // Wait for some 2 seconds
        yield return new WaitForSecondsRealtime(2f);

        // stop time
        Time.timeScale = 0;
    }
}
