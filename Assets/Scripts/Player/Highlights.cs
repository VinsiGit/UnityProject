using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlights : MonoBehaviour
{
    public Material highlightMat;
    Material originalMat;
    GameObject prevHighlight;


    void ClearHighlighted()
    {
        // clear material from last looked at material
        if (prevHighlight != null)
        {
            prevHighlight.GetComponent<MeshRenderer>().sharedMaterial = originalMat;
            prevHighlight = null;
        }
    }

    void HighlightObject(GameObject gameObject)
    {
        if (prevHighlight != gameObject)
        {
            ClearHighlighted();
            originalMat = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = highlightMat;
            prevHighlight = gameObject;
        }

    }

    void DetectObject()
    {
        // detect object with raycast
        float rayDistance = 2.5f;
        // Ray from the center of the viewport.
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit rayHit;
        // Check if we hit something.
        if (Physics.Raycast(ray, out rayHit, rayDistance))
        {
            // Get the object that was hit.
            GameObject hitObject = rayHit.collider.gameObject;
            HighlightObject(hitObject);
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