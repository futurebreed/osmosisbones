using System;
using UnityEngine;

public class RailManager : MonoBehaviour
{
    [SerializeField]
    private Node[] nodes;

    [SerializeField]
    private float offsetRadius;

    public Node[] GetNodes()
    {
        return nodes;
    }

    public float GetOffsetMax()
    {
        return offsetRadius;
    }

    [Serializable]
    public struct Node
    {
        public Vector3 position;
        public Vector3 worldUp;
    }
}
