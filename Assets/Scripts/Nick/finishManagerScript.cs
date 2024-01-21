using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishManagerScript : MonoBehaviour
{

    public GameObject finishUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Finished()
    {
        finishUI.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("starting_menu");
    }

    public void quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
