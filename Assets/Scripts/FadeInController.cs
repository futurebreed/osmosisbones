using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInController : MonoBehaviour
{
    [SerializeField]
    private float fadeDelay;

    [SerializeField]
    private float fadeAmount;

    private Coroutine fadeInRoutine;
    private Image objectImage;

    private void Start()
    {
        objectImage = GetComponent<Image>();

        // Create and start the fade-in coroutine
        fadeInRoutine = StartCoroutine(FadeIn());
    }

    private IEnumerator<WaitForSeconds> FadeIn()
    {
        float objectAlpha = 0f;
        while (objectAlpha < 1f)
        {
            var color = objectImage.color;
            objectImage.color = new Color(color.r, color.g, color.b, color.a + fadeAmount);

            yield return new WaitForSeconds(fadeDelay * Time.deltaTime);
        }

        StopFade();
    }

    public void StopFade()
    {
        if (fadeInRoutine != null)
        {
            StopCoroutine(fadeInRoutine);
        }
    }
}