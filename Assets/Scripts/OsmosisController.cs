using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsmosisController : MonoBehaviour
{
    [SerializeField]
    private float sideSpeed;

    // RailController is attached to an empty object that moves along the rail
    [SerializeField]
    private RailController railController;

    [SerializeField]
    private RailManager railManager;

    private Vector2 offset = Vector2.zero;
    private float offsetMax;

    private void Start()
    {
        offsetMax = railManager.GetOffsetMax();
    }

    private void Update()
    {
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        offset += new Vector2(horz, vert) * Time.deltaTime * sideSpeed;
        offset = BoundCircle(offset, offsetMax);
        Transform railTransform = railController.UpdateRail();
        transform.position = railTransform.position + (railTransform.rotation * new Vector3(offset.x, offset.y, 0f));
        transform.rotation = railTransform.rotation;
    }

    private float BoundValues(float realValue, float max)
    {
        return Mathf.Sign(realValue) > 0 ? Mathf.Min(realValue, max) : Mathf.Max(realValue, -max);
    }

    // If the given offset's magnitude is greater than the offsetMax radius, return a new offset where the magnitude is the offsetMax
    private Vector2 BoundCircle(Vector2 point, float radius)
    {
        if (offset.sqrMagnitude > radius * radius)
        {
            return Vector2.MoveTowards(Vector2.zero, point, radius);
        }
        return point;
    }
}
