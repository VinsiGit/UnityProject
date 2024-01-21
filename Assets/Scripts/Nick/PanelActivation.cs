using UnityEngine;

public class PanelActivation : MonoBehaviour
{
    public GameObject selectedCanvas;
    public finishManagerScript finishManager;

    void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider has the "finish" tag
        if (other.CompareTag("finish"))
        {
            // Enable the selected panel
            if (selectedCanvas != null)
            {
                
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
