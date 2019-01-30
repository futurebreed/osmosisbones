using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OsmosisController : MonoBehaviour
{
    [SerializeField]
    private float sideSpeed;

    [SerializeField]
    private float speedUp;

    [SerializeField]
    private float speedUpDuration;

    [SerializeField]
    private float fireRest = 0.5f;

    [SerializeField]
    private Vector3 bulletOffset = new Vector3(0f, 0f, 5f);

    // RailController is attached to an empty object that moves along the rail
    [SerializeField]
    private RailController railController;

    [SerializeField]
    private RailManager railManager;

    [SerializeField]
    private PromptManager promptManager;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private GameObject fadeObject;

    [SerializeField]
    private float fadeDelay;

    private ObjectPooler bulletPool;

    private Vector2 offset = Vector2.zero;
    private float offsetMax;

    private float timeSinceRedHit;
    private float timeSinceLastFire = float.MinValue;

    private Coroutine fadeToCredits;

    private void Start()
    {
        offsetMax = railManager.GetOffsetMax();
        timeSinceRedHit = float.MinValue;

        audioManager.PlayBackgroundAudio(Guid.Parse("4fa9cb90-d08b-455a-9635-6e9ba4abb6bf"), gameObject, .0005f);
        bulletPool = GetComponent<ObjectPooler>();
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
            audioManager.Stop();
            promptManager.HidePrompt();
            SceneManager.LoadScene((int)Scenes.MainMenu);
        }
        if (Input.GetButton("Fire1") && Time.time - timeSinceLastFire > fireRest)
        {
            timeSinceLastFire = Time.time;
            BulletController bullet = bulletPool.GetObject(transform.position + (transform.rotation * bulletOffset), 
                transform.rotation).GetComponent<BulletController>();
            bullet.SetInitialValues(railController.GetRailSpeed(), bulletPool);
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
        if (other.CompareTag("Cell") || other.CompareTag("Plaque"))
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
        else if (other.CompareTag("MusicFade"))
        {
            audioManager.FadeBackgroundAudio(-0.0005f);

            // Make sure any prompts get hidden
            promptManager.HidePrompt();
        }
        else if (other.CompareTag("Heart"))
        {
            Debug.Log("Hit heart!");
            fadeToCredits = StartCoroutine(FadeToCredits());
        }
    }

    private IEnumerator<WaitForSeconds> FadeToCredits()
    {
        Image fadeImage = fadeObject.GetComponent<Image>();
        while (fadeImage.color.a < 1f)
        {
            var color = fadeImage.color;
            fadeImage.color = new Color(color.r, color.g, color.b, color.a + fadeDelay);

            yield return new WaitForSeconds(fadeDelay * Time.deltaTime);
        }

        SceneManager.LoadScene((int)Scenes.Credits);
    }
}