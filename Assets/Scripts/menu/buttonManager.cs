using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonManager : MonoBehaviour
{
    public GameObject main;
    public GameObject info;
    // Start is called before the first frame update
    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowInfo()
    {
        main.gameObject.SetActive(false);
        info.gameObject.SetActive(true);
    }

    public void ReturnToMain()
    {
        info.gameObject.SetActive(false);
        main.gameObject.SetActive(true);
    }


    // Update is called once per frame
    public void Quit()
    {
        Application.Quit();
    }
}
