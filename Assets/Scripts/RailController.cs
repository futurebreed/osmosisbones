﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should be own gameobject so the Transform is not affected by the offset
public class RailController : MonoBehaviour
{
    [SerializeField]
    private RailManager railManager;

    [SerializeField]
    private float railSpeed;

    [SerializeField]
    private float turningDistance = 3f;

    // Camera could just be a child of the RailTransform. Might be simpler
    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private Vector3 cameraOffset;

    private int index = 0;

    private float angleStep = 0;
    private Quaternion lookRotation;
    private bool isTurning = false;

    private void Start()
    {
        transform.LookAt(railManager.GetNodes()[0]);
    }

    public Transform UpdateRail()
    {
        Vector3 currentNode = railManager.GetNodes()[index];
        transform.position = Vector3.MoveTowards(transform.position, currentNode, railSpeed * Time.deltaTime);
        Vector3 directionVector = currentNode - transform.position;
        if (directionVector.sqrMagnitude == 0f)
        {
            isTurning = false;
            index++;
            if (index == railManager.GetNodes().Length)
            {
                index = 0;
            }
            transform.LookAt(railManager.GetNodes()[index]);
            
        }
        // Try to turn towards the next vector smoothly
        else if (directionVector.sqrMagnitude < (turningDistance * turningDistance) && index + 1 != railManager.GetNodes().Length)
        {
            if (isTurning)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, angleStep * Time.deltaTime);
            }
            else
            {
                // Get the end rotation
                lookRotation = Quaternion.LookRotation(railManager.GetNodes()[index + 1] - currentNode);

                // Get the angle we need to turn and the distance we have left
                float angleRemaining = Quaternion.Angle(transform.rotation, lookRotation);
                float distanceRemaining = directionVector.magnitude; // Need the actual magnitude :(

                // Calculate how much we need to turn each second
                angleStep = (angleRemaining / distanceRemaining) * railSpeed;
                Debug.LogFormat("AngleStep set to {0}", angleStep);                
                isTurning = true;
            }

        }
        cameraTransform.position = transform.position + (transform.rotation * cameraOffset);
        cameraTransform.rotation = transform.rotation;
        return transform;
    }

    public void resetRail()
    {
        index = 0;
        transform.position = Vector3.zero;
        transform.LookAt(railManager.GetNodes()[0]);
    }
}
