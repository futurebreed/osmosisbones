using System;
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

    private void Start()
    {
        // Make sure cells don't spawn right next to the start position
        // What determines start position? Currently just using Vector3.zero
        Vector3 currentPosition = Vector3.MoveTowards(Vector3.zero, railManager.GetNodes()[0].position, 5f);
        float offsetMax = railManager.GetOffsetMax();
        // Traverse through the nodes
        foreach (RailManager.Node node in railManager.GetNodes())
        {
            if (node.cinematic)
            {
                currentPosition = node.position;
                continue;
            }
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
                            Instantiate(spawnerElement.prefab, offset, Quaternion.identity);
                        }
                        else
                        {
                            int spawnCount = (int)UnityEngine.Random.Range(0.80f * spawnerElement.burstFactor, 
                                1.20f * spawnerElement.burstFactor);
                            for (int i = 0; i < spawnCount; i++)
                            {
                                Vector3 burstOffset = offset + (UnityEngine.Random.rotationUniform *
                                    (Vector3.up * UnityEngine.Random.Range(0, spawnerElement.burstDistance)));

                                GameObject obj = Instantiate(spawnerElement.prefab, burstOffset, Quaternion.identity);
                                obj.GetComponent<Renderer>().material.color = Color.red;
                            }
                        }
                    }
                }
            }
        }
    }

    [Serializable]
    private struct SpawnerElement
    {
        public GameObject prefab;

        [Range(0, 100)]
        public float spawnChance;
        public bool burst;
        public int burstFactor;
        public float burstDistance;
    }
}
