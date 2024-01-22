using TMPro;
using UnityEngine;

public class PanelActivation : MonoBehaviour
{
    public GameObject selectedCanvas;
    public finishManagerScript finishManager;
    private AudioListener audioListener;
    public TextMeshProUGUI tmpUI;

    void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider has the "finish" tag
        if (other.CompareTag("finish"))
        {
            // Enable the selected panel
            if (selectedCanvas != null)
            {
                tmpUI.text = tmpUI.text + $"{PlayerManager.Score}";
                AudioListener.volume = 0f;
                selectedCanvas.SetActive(false);
                Invoke("ActivatePanel", 1);
                
            }
        }
    }
    void ActivatePanel()
    {
        float scale = 1.0f;
        finishManager.Finished();
        while (scale > 0.01)
        {
            scale *= 0.99f;
            Time.timeScale = scale;
        }
        Time.timeScale = 0f;
        
    }
}
