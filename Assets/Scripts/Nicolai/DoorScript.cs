using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject collisionObject;
    public Transform doorTransform;  // Reference to the transform of the door object
    public float downDistance = 2.0f;  // Distance to move the door down
    public float moveDuration = 1.0f;  // Time it takes to move the door down
    public float returnDoorDelay = 5.0f;  // Delay before returning the door
    public AudioClip doorAudioClip;  // Sound effect for the door

    private bool isMoving = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == collisionObject && !isMoving)
        {
            StartCoroutine(MoveDoor());
        }
    }

    IEnumerator MoveDoor()
    {
        isMoving = true;

        // Play the door sound
        if (doorAudioClip != null)
        {
            AudioSource.PlayClipAtPoint(doorAudioClip, doorTransform.position);
        }

        // Move the door down
        Vector3 originalPosition = doorTransform.position;
        Vector3 targetPosition = originalPosition - new Vector3(0f, downDistance, 0f);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            doorTransform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        doorTransform.position = targetPosition;

        // Wait for specified seconds
        yield return new WaitForSeconds(returnDoorDelay);

        // Move the door back up
        elapsedTime = 0f;
        if (elapsedTime < moveDuration)
        {
            AudioSource.PlayClipAtPoint(doorAudioClip, doorTransform.position);
        }

        while (elapsedTime < moveDuration)
        {
            doorTransform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        doorTransform.position = originalPosition;

        isMoving = false;
    }
}