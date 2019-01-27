using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{

    private float rotationSpeed;

    private void Start()
    {
        transform.Rotate(new Vector3(1f, 0f), Random.Range(0f, 90f));
        rotationSpeed = Random.Range(360f, 720f);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0f, rotationSpeed * Time.deltaTime, 0f), Space.World);
    }
}
