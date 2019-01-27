using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private RailManager railManager;

    [SerializeField]
    private GameObject cellPrefab;

    [SerializeField]
    private float stepDistance;

    [SerializeField]
    private SpawnerElement[] spawnElements;

    private int i = 0;
    private int lastIndexCleared = 0;
    private Vector3 currentPosition;

    private void Start()
    {
        //Hide the cursor
        Cursor.visible = false;

        // Make sure cells don't spawn right next to the start position
        // What determines start position? Currently just using Vector3.zero
        currentPosition = Vector3.MoveTowards(Vector3.zero, railManager.GetNodes()[0].position, 5f);

        for (int index = 0; index < spawnElements.Length; index++)
        {
            spawnElements[index].spawnedObjects = new List<List<GameObject>>();
            for (int j = 0; j < railManager.GetNodes().Length; j++)
            {
                spawnElements[index].spawnedObjects.Add(new List<GameObject>());
            }
        }

        // Traverse through the nodes
        while (i < 3)
        {
            InstantiateNextNode();
        }
    }

    public void InstantiateNextNode()
    {
        if (i >= railManager.GetNodes().Length)
        {
            return;
        }
        RailManager.Node node = railManager.GetNodes()[i];
        if (node.cinematic)
        {
            currentPosition = node.position;
            i++;
            return;
        }
        float offsetMax = railManager.GetOffsetMax();
        // Loop until we are close enough to move on to the next node
        while ((node.position - currentPosition).sqrMagnitude != 0f)
        {
            // Get the direction towards the node before we move towards it
            Vector3 direction = node.position - currentPosition;
            // Move towards node at the given "StepDistance"
            currentPosition = Vector3.MoveTowards(currentPosition, node.position, stepDistance);

            foreach (SpawnerElement spawnerElement in spawnElements)
            {
                // Roll to see if we should spawn a cell
                if (UnityEngine.Random.Range(0, 100) < spawnerElement.spawnChance)
                {
                    // Get the forward vector's rotation
                    Quaternion rotation = Quaternion.LookRotation(direction);

                    // Roll a random angle and return the rotation
                    Quaternion randomAngle = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.forward);

                    // Roll a random radius (distance from the rail)
                    Vector3 randomRadius = new Vector3(0f, UnityEngine.Random.Range(0f, offsetMax));

                    // apply the random angle to the random radius, then apply the forward vector.
                    Vector3 offset = currentPosition + (rotation * (randomAngle * randomRadius));
                    if (!spawnerElement.burst)
                    {
                        spawnerElement.spawnedObjects[i].Add(spawnerElement.prefabPooler.GetObject(offset, Quaternion.identity));
                        //Instantiate(spawnerElement.prefab, offset, Quaternion.identity);
                    }
                    else
                    {
                        int spawnCount = (int)UnityEngine.Random.Range(0.80f * spawnerElement.burstFactor,
                            1.20f * spawnerElement.burstFactor);
                        for (int j = 0; j < spawnCount; j++)
                        {
                            Vector3 burstOffset = offset + (UnityEngine.Random.rotationUniform *
                                (Vector3.up * UnityEngine.Random.Range(0, spawnerElement.burstDistance)));

                            spawnerElement.spawnedObjects[i].Add(
                                spawnerElement.prefabPooler.GetObject(burstOffset, Quaternion.identity));
                            //GameObject obj = Instantiate(spawnerElement.prefab, burstOffset, Quaternion.identity);
                        }
                    }
                }
            }
        }
        i++;
    }

    public void ClearUpToIndex(int newIndex)
    {
        for (; lastIndexCleared < newIndex; lastIndexCleared++)
        {
            foreach (SpawnerElement spawnerElement in spawnElements)
            {
                foreach (GameObject obj in spawnerElement.spawnedObjects[lastIndexCleared])
                {
                    spawnerElement.prefabPooler.ReleaseObject(obj);
                }
            }
        }
    }

    [Serializable]
    private struct SpawnerElement
    {
        public ObjectPooler prefabPooler;
        public List<List<GameObject>> spawnedObjects;

        [Range(0, 100)]
        public float spawnChance;
        public bool burst;
        public int burstFactor;
        public float burstDistance;
    }
}
