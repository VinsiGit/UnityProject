using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class itemHighlight : MonoBehaviour
{
    public Material highlightMat;
    public TextMeshProUGUI interactionText; // Reference to TextMeshProUGUI component

    Material[] originalMaterials;
    GameObject prevHighlight;

    public void ClearHighlighted()
    {
        interactionText.gameObject.SetActive(false);
        // Clear material from the last looked at material
        if (prevHighlight != null)
        {
            if (originalMaterials != null)
            {
                // Restore the original materials
                prevHighlight.GetComponent<Renderer>().materials = originalMaterials;
            }
            prevHighlight = null;
            // Hide the interaction text when not highlighted
        }
    }

    void HighlightObject(GameObject gameObject)
    {
        if (prevHighlight != gameObject)
        {
            ClearHighlighted();

            // Store the original materials
            originalMaterials = gameObject.GetComponent<Renderer>().materials;

            // Create a new materials array with the highlight material
            Material[] newMaterials = new Material[originalMaterials.Length];
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                newMaterials[i] = originalMaterials[i];
            }

            newMaterials[newMaterials.Length - 1] = highlightMat;

            // Apply the new materials array
            gameObject.GetComponent<Renderer>().materials = newMaterials;

            prevHighlight = gameObject;

            // Show the interaction text
            interactionText.gameObject.SetActive(true);
        }
    }

    void DetectObject()
    {
        // Detect object with raycast
        float rayDistance = 2.5f;

        // Ray from the center of the viewport.
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit rayHit;

        // Check if we hit something.
        if (Physics.Raycast(ray, out rayHit, rayDistance))
        {
            // Get the object that was hit.
            GameObject hitObject = rayHit.collider.gameObject;
            if (hitObject.tag == "Item")
            {
                HighlightObject(hitObject);
            }
            else
            {
                ClearHighlighted();
            }
        }
        else
        {
            ClearHighlighted();
        }
    }

    void Update()
    {
        DetectObject();
    }
}
