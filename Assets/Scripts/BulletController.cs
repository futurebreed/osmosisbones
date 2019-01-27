using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    public float extraSpeed = 20f;

    [SerializeField]
    private float ttl = 2;

    private float shipSpeed;
    private float timeStart;
    private ObjectPooler bulletPool;
    private bool checkTtl = false;

    public void SetInitialValues(float shipSpeed, ObjectPooler bulletPool)
    {
        this.shipSpeed = shipSpeed;
        this.bulletPool = bulletPool;
        timeStart = Time.time;
        checkTtl = true;
    }

    private void Update()
    {
        if (checkTtl && Time.time - timeStart > ttl)
        {
            checkTtl = false;
            bulletPool.ReleaseObject(gameObject);
            return;
        }

        transform.Translate(Vector3.forward * (shipSpeed + extraSpeed) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plaque"))
        {
            checkTtl = false;
            bulletPool.ReleaseObject(gameObject);
        }
    }
}
