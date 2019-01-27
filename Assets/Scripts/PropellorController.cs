using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellorController : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotationStep;

    private void Update()
    {
        transform.Rotate(rotationStep * Time.deltaTime);
    }
}
