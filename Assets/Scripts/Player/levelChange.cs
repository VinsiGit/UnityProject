using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelChange : MonoBehaviour
{

    public StateManager statemanager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void DetectObject()
    {
        // Detect object with raycast
        float rayDistance = 2.5f;

        // ray from view
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit rayHit;

        // Check hit
        if (Physics.Raycast(ray, out rayHit, rayDistance))
        {
            // Get the object that was hit.
            GameObject hitObject = rayHit.collider.gameObject;
            if (hitObject.tag == "Exit")
            {
                if (Input.GetKey(KeyCode.E))
                {
                    statemanager.loadNextLevel();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectObject();
    }
}
