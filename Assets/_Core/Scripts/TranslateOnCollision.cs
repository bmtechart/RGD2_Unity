using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TranslateOnCollision : MonoBehaviour
{
    // The object that will be moved
    public GameObject objectToMove;

    // The start and end positions and rotations for the movement
    public GameObject startPositionObject;
    public GameObject endPositionObject;

    // Duration of the movement in seconds
    public float movementDuration = 2f;

    // Buffer period in seconds to prevent re-triggering
    public float triggerBuffer = 1f;

    // Tag of the object that will trigger the movement
    public string triggerTag = "MoveObjectTag";

    // State flag to determine if the object is at the start or end position
    private bool atStartPosition = true;

    // Flag to check if the object is currently moving
    private bool isMoving = false;

    // Flag to control whether the object can move back to the start position
    public bool allowReTriggering = true;

    void Start()
    {
        // Set the object at the start position and rotation initially
        SetToStartPosition();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the specified tag and the object is not currently moving
        if (collision.gameObject.CompareTag(triggerTag) && !isMoving)
        {
            // Check if re-triggering is allowed or if the object is at the start position
            if (allowReTriggering || atStartPosition)
            {
                // Start the movement and rotation coroutine
                StartCoroutine(MoveAndRotateObject());
            }
        }
    }

    IEnumerator MoveAndRotateObject()
    {
        isMoving = true;

        float elapsedTime = 0;
        Vector3 startPosition = atStartPosition ? startPositionObject.transform.position : endPositionObject.transform.position;
        Vector3 endPosition = atStartPosition ? endPositionObject.transform.position : startPositionObject.transform.position;
        Quaternion startRotation = atStartPosition ? startPositionObject.transform.rotation : endPositionObject.transform.rotation;
        Quaternion endRotation = atStartPosition ? endPositionObject.transform.rotation : startPositionObject.transform.rotation;

        while (elapsedTime < movementDuration)
        {
            objectToMove.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / movementDuration));
            objectToMove.transform.rotation = Quaternion.Lerp(startRotation, endRotation, (elapsedTime / movementDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectToMove.transform.position = endPosition;
        objectToMove.transform.rotation = endRotation;

        // Toggle position state only if re-triggering is allowed
        if (allowReTriggering || atStartPosition)
        {
            atStartPosition = !atStartPosition;
        }

        // Wait for the buffer period before allowing another trigger
        yield return new WaitForSeconds(triggerBuffer);
        isMoving = false;
    }

    private void SetToStartPosition()
    {
        if (objectToMove != null && startPositionObject != null)
        {
            objectToMove.transform.position = startPositionObject.transform.position;
            objectToMove.transform.rotation = startPositionObject.transform.rotation;
        }
    }
}

