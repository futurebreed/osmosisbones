using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private int startCount;

    private Queue<GameObject> pool;

    private void Start()
    {
        pool = new Queue<GameObject>();
        for (int i = 0; i < startCount; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        GameObject obj;
        if (pool.Count != 0)
        {
            obj = pool.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab, position, rotation);
        }
        return obj;
    }

    public void ReleaseObject(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
