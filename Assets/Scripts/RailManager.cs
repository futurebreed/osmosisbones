using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailManager : MonoBehaviour
{
    [SerializeField]
    private Vector3[] nodes;

    [SerializeField]
    private float offsetRadius;

    public Vector3[] GetNodes()
    {
        return nodes;
    }

    public float GetOffsetMax()
    {
        return offsetRadius;
    }
}
