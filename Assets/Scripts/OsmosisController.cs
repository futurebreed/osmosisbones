using UnityEngine;
using UnityEngine.SceneManagement;

public class OsmosisController : MonoBehaviour
{
    [SerializeField]
    private float sideSpeed;

    [SerializeField]
    private float speedUp;

    [SerializeField]
    private float speedUpDuration;

    // RailController is attached to an empty object that moves along the rail
    [SerializeField]
    private RailController railController;

    [SerializeField]
    private RailManager railManager;

    [SerializeField]
    private PromptManager promptManager;

    [SerializeField]
    private FMODUnity.StudioEventEmitter soundEmitter;

    private Vector2 offset = Vector2.zero;
    private float offsetMax;

    private float timeSinceRedHit;

    private void Start()
    {
        offsetMax = railManager.GetOffsetMax();
        timeSinceRedHit = float.MinValue;
        soundEmitter.Play();
    }

    private void Update()
    {
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        float speed = sideSpeed + (Time.time - timeSinceRedHit > speedUpDuration ? 0 : speedUp); 
        offset += new Vector2(horz, vert) * Time.deltaTime * speed;
        offset = BoundCircle(offset, offsetMax);
        Transform railTransform = railController.UpdateRail();
        transform.position = railTransform.position + (railTransform.rotation * new Vector3(offset.x, offset.y, 0f));
        transform.rotation = railTransform.rotation;

        if(Input.GetButtonUp("Quit"))
        {
            Debug.Log("Go to Menu");
            SceneManager.LoadScene((int)Scenes.MainMenu);
            soundEmitter.Stop();
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cell"))
        {
            // Reset path
            railController.ResetRail();
            timeSinceRedHit = float.MinValue;
            promptManager.ShowRespawnPrompt();
        }
        else if (other.CompareTag("RedCell"))
        {
            Debug.Log("Hit red cell!");
            timeSinceRedHit = Time.time;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Heart"))
        {
            Debug.Log("Hit heart!");
            SceneManager.LoadScene((int)Scenes.Credits);
        }
    }
}
