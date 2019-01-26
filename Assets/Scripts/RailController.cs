using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailController : MonoBehaviour
{
    [SerializeField]
    private Vector3[] nodes;

    [SerializeField]
    private float railSpeed;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private Vector3 cameraOffset;

    private int index = 0;
    private Vector3 currentPosition;

    private void Start()
    {
        currentPosition = transform.position;
    }

    public Vector3 UpdateRail()
    {
        Vector3 currentNode = nodes[index];
        currentPosition = Vector3.MoveTowards(currentPosition, currentNode, railSpeed * Time.deltaTime);               

        if ((currentNode - currentPosition).sqrMagnitude < 0.5f)
        {
            index++;
            if (index == nodes.Length)
            {
                index = 0;
            }
            transform.LookAt(nodes[index]);
        }
        cameraTransform.position = currentPosition + (transform.rotation * cameraOffset);
        cameraTransform.rotation = transform.rotation;
        return currentPosition;
    }
}
