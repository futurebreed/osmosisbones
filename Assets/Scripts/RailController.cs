using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should be own gameobject so the Transform is not affected by the offset
public class RailController : MonoBehaviour
{
    [SerializeField]
    private RailManager railManager;

    [SerializeField]
    private PromptManager promptManager;

    [SerializeField]
    private Spawner spawner;

    [SerializeField]
    private float railSpeed;

    [SerializeField]
    private float speedIncrease;

    [SerializeField]
    private float cinematicSpeed;

    [SerializeField]
    private float turningDistance = 3f;

    // Camera could just be a child of the RailTransform. Might be simpler
    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private Vector3 cameraOffset;

    private int index = 0;
    private int checkpoint = 0;

    private float angleStep = 0;
    private Quaternion lookRotation;
    private bool isTurning = false;

    private void Start()
    {
        transform.LookAt(railManager.GetNodes()[0].position, railManager.GetNodes()[0].worldUp);
        promptManager.ShowStoryPrompt();
    }

    private void Update()
    {
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.G))
        {
            index = 18;
            transform.position = railManager.GetNodes()[index].position;
            promptManager.HidePrompt();
        }
    }

    public Transform UpdateRail()
    {
        RailManager.Node currentNode = railManager.GetNodes()[index];
        float speed = currentNode.cinematic ? cinematicSpeed : railSpeed;
        transform.position = Vector3.MoveTowards(transform.position, currentNode.position, speed * Time.deltaTime);
        Vector3 directionVector = currentNode.position - transform.position;
        if (directionVector.sqrMagnitude == 0f)
        {
            spawner.InstantiateNextNode();
            isTurning = false;
            index++;
            if (index < railManager.GetNodes().Length)
            {
                transform.LookAt(railManager.GetNodes()[index].position, railManager.GetNodes()[index].worldUp);
                if (railManager.GetNodes()[index].cinematic)
                {
                    checkpoint = index;
                    spawner.ClearUpToIndex(checkpoint);

                    if (index < 19)
                    {
                        promptManager.ShowStoryPrompt();
                    }

                    railSpeed += speedIncrease;
                }
            }
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
                RailManager.Node nextNode = railManager.GetNodes()[index + 1];
                lookRotation = Quaternion.LookRotation(nextNode.position - currentNode.position, nextNode.worldUp);

                // Get the angle we need to turn and the distance we have left
                float angleRemaining = Quaternion.Angle(transform.rotation, lookRotation);
                float distanceRemaining = directionVector.magnitude; // Need the actual magnitude :(

                // Calculate how much we need to turn each second
                angleStep = (angleRemaining / distanceRemaining) * speed;
                Debug.LogFormat("AngleStep set to {0}", angleStep);                
                isTurning = true;
            }

        }
        cameraTransform.position = transform.position + (transform.rotation * cameraOffset);
        cameraTransform.rotation = transform.rotation;
        return transform;
    }

    public void ResetRail()
    {
        index = checkpoint;
        if (index - 1 > -1)
        {
            transform.position = railManager.GetNodes()[index - 1].position;
        } else
        {
            transform.position = Vector3.zero;
        }
        transform.LookAt(railManager.GetNodes()[index].position, railManager.GetNodes()[index].worldUp);
        spawner.RespawnPlaque(index);
    }

    public float GetRailSpeed()
    {
        return railSpeed;
    }
}
