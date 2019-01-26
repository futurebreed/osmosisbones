using System.Collections;
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

    [SerializeField, Range(0, 100)]
    private float stepChance;

    private void Start()
    {
        // Make sure cells don't spawn right next to the start position
        // What determines start position? Currently just using Vector3.zero
        Vector3 currentPosition = Vector3.MoveTowards(Vector3.zero, railManager.GetNodes()[0], 5f);
        float offsetMax = railManager.GetOffsetMax();
        // Traverse through the nodes
        foreach (Vector3 node in railManager.GetNodes())
        {
            // Loop until we are close enough to move on to the next node
            while ((node - currentPosition).sqrMagnitude > 0.1f)
            {
                // Get the direction towards the node before we move towards it
                Vector3 direction = node - currentPosition;
                // Move towards node at the given "StepDistance"
                currentPosition = Vector3.MoveTowards(currentPosition, node, stepDistance);

                // Roll to see if we should spawn a cell
                if (Random.Range(0, 100) < stepChance)
                {
                    // Get the forward vector's rotation
                    Quaternion rotation = Quaternion.LookRotation(direction);

                    // Roll a random angle and return the rotation
                    Quaternion randomAngle = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

                    // Roll a random radius (distance from the rail)
                    Vector3 randomRadius = new Vector3(0f, Random.Range(0f, offsetMax));

                    // apply the random angle to the random radius, then apply the forward vector.
                    Vector3 offset = rotation * (randomAngle * randomRadius);
                    Instantiate(cellPrefab, currentPosition + offset, Quaternion.identity);
                }
            }
        }
    }
}
