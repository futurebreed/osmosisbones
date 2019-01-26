using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should be own gameobject so the Transform is not affected by the offset
public class RailController : MonoBehaviour
{
    [SerializeField]
    private RailManager railManager;

    [SerializeField]
    private float railSpeed;

    // Camera could just be a child of the RailTransform. Might be simpler
    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private Vector3 cameraOffset;

    private int index = 0;

    private void Start()
    {
        transform.LookAt(railManager.GetNodes()[0]);
    }

    public Transform UpdateRail()
    {
        Vector3 currentNode = railManager.GetNodes()[index];
        transform.position = Vector3.MoveTowards(transform.position, currentNode, railSpeed * Time.deltaTime);               

        if ((currentNode - transform.position).sqrMagnitude < 0.01f)
        {
            index++;
            if (index == railManager.GetNodes().Length)
            {
                index = 0;
            }
            transform.LookAt(railManager.GetNodes()[index]);
        }
        cameraTransform.position = transform.position + (transform.rotation * cameraOffset);
        cameraTransform.rotation = transform.rotation;
        return transform;
    }
}
