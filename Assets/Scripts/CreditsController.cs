using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    [SerializeField]
    FMODUnity.StudioEventEmitter soundEmitter;

    [SerializeField]
    private GameObject fadeInObject;

    [SerializeField]
    private float fadeDelay;

    [SerializeField]
    private float fadeWaitToStart;

    private Coroutine fadeInRoutine;

    private void Start()
    {
        soundEmitter.Play();
        fadeInRoutine = StartCoroutine(FadeIn());
    }

    private void Update()
    {
        bool aButtonPressed = Input.GetButtonUp("Fire1");
        bool quitButtonPressed = Input.GetButtonUp("Quit");
        if (aButtonPressed || quitButtonPressed)
        {
            SceneManager.LoadScene((int)Scenes.MainMenu);
            soundEmitter.Stop();
        }
    }

    private IEnumerator<WaitForSeconds> FadeIn()
    {
        Image objectImage = fadeInObject.GetComponent<Image>();

        yield return new WaitForSeconds(fadeWaitToStart);

        while (objectImage.color.a > 0f)
        {
            var color = objectImage.color;
            objectImage.color = new Color(color.r, color.g, color.b, color.a - fadeDelay);

            yield return new WaitForSeconds(fadeDelay * Time.deltaTime);
        }
    }
}