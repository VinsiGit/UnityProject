using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    public PlayerManager playerManager;
    public UIManager UiManager;
    public itemHighlight highlightscript;
    public AudioSource src;
    public AudioClip pickup;
    public AudioClip store;

    public float offset_x = 0;
    public float offset_y = -1.2f;
    public float offset_z = 1;
    public float tilt = 10;

    private GameObject heldItem; // Reference to the currently held item
    private bool isHolding = false; // Flag to determine if the player can pick up more items -> add to player manager

    void PickupObject(GameObject gameObject)
    {
        // Check if the player can pick up more items
        if (isHolding)
            return;

        // clear the highlight
        highlightscript.ClearHighlighted();

        // Destroy the object
        Destroy(gameObject);

        src.PlayOneShot(pickup);

        ShowHeldItem(gameObject);

        // Disable pickups when holding shit
        isHolding = true;

        UiManager.InteractionTextActive(false);
    }

    void ShowHeldItem(GameObject gameObject)
    {
        Transform playerCamera = transform.Find("Main Camera");
        Transform bat = playerCamera.transform.Find("weapon");
        bat.gameObject.SetActive(false);

        if (playerCamera != null)
        {
            // Instantiate the object as a child of the player's camera
            heldItem = Instantiate(gameObject, playerCamera.position, Quaternion.identity);
            // disable collider
            BoxCollider boxCollider = heldItem.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.enabled = false;
            }
            // remove item tag
            heldItem.tag = "Untagged";
            // add to camera
            heldItem.transform.parent = playerCamera;
            // change held item relative position
            heldItem.transform.localRotation = Quaternion.Euler(tilt, 0, 0);
            heldItem.transform.localPosition = new Vector3(offset_x, offset_y, offset_z);
        }
    }

    void StoreItem()
    {
        // Check if the player is holding an item
        if (heldItem != null)
        {
            // Destroy the held item
            Destroy(heldItem);

            src.PlayOneShot(store);

            playerManager.addScore(1);

            // reactivate bat
            Transform playerCamera = transform.Find("Main Camera");
            Transform bat = playerCamera.transform.Find("weapon");
            bat.gameObject.SetActive(true);

            // enable pickips again
            isHolding = false;
        }
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
            if (hitObject.tag == "Item")
            {
                if (Input.GetKey(KeyCode.E))
                {
                    PickupObject(hitObject);
                }
            }
            else if (hitObject.tag == "Container")
            {
                if ((Input.GetKey(KeyCode.E)) && (isHolding == true))
                {
                    StoreItem();
                }
            }
        }
    }

    void Update()
    {
        DetectObject();
    }
}
