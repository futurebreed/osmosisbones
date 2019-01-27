using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    [SerializeField]
    private float scaleMin;

    [SerializeField]
    private float scaleMax;

    [SerializeField]
    private float scaleSpeed;

    [SerializeField]
    private float scaleRest;

    private float currentScale;

    private bool increase = true;
    private bool isWaiting = false;
    private float timeSinceBeat;

    private void Start()
    {
        currentScale = scaleMin;
        transform.localScale = new Vector3(scaleMin, scaleMin, scaleMin);
    }

    private void Update()
    {
        if (isWaiting)
        {
            if (Time.time - timeSinceBeat > scaleRest)
            {
                isWaiting = false;
            }
            else
            {
                return;
            }
        }
        if (increase)
        {
            currentScale = Mathf.Min(scaleMax, currentScale + (scaleSpeed * Time.deltaTime));
            if (currentScale == scaleMax)
            {
                increase = false;
            }
        }
        else
        {
            currentScale = Mathf.Max(scaleMin, currentScale - (scaleSpeed * Time.deltaTime));
            if (currentScale == scaleMin)
            {
                increase = true;
                isWaiting = true;
                timeSinceBeat = Time.time;
            }
        }
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }
}
