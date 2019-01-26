using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsmosisController : MonoBehaviour
{
    [SerializeField]
    private float sideSpeed;

    [SerializeField]
    private Vector2 offsetMax;

    private Vector2 offset = Vector2.zero;
    private RailController railController;

    private void Start()
    {
        railController = GetComponent<RailController>();
    }

    private void Update()
    {
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        offset += new Vector2(horz, vert) * Time.deltaTime * sideSpeed;
        offset = new Vector2(BoundValues(offset.x, offsetMax.x), BoundValues(offset.y, offsetMax.y));
        
        transform.position = railController.UpdateRail() + (transform.rotation * new Vector3(offset.x, offset.y, 0f));
    }

    private float BoundValues(float realValue, float max)
    {
        return Mathf.Sign(realValue) > 0 ? Mathf.Min(realValue, max) : Mathf.Max(realValue, -max);
    }
}
