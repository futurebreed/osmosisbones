using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAndPulseController : MonoBehaviour
{
    [SerializeField]
    private float fadeDelay;

    [SerializeField]
    private float fadeAmount;

    [SerializeField]
    private float pulseIntensity;

    [SerializeField]
    private float maxPulse;

    private Coroutine fadeInRoutine;
    private Image objectImage;

    private void Start()
    {
        objectImage = GetComponent<Image>();

        // Create and start the fade-in coroutine
        fadeInRoutine = StartCoroutine(FadeInAndPulse());
    }

    private IEnumerator<WaitForSeconds> FadeInAndPulse()
    {
        while (objectImage.color.a < 1f)
        {
            var color = objectImage.color;
            objectImage.color = new Color(color.r, color.g, color.b, color.a + fadeAmount);

            yield return new WaitForSeconds(fadeDelay * Time.deltaTime);
        }

        var maxPulseVector = transform.localScale.x + maxPulse;
        var minPulseVector = transform.localScale.x - maxPulse;
        var pulseIntensityVector = new Vector3(pulseIntensity, 0, 0);

        // True = larger, false = smaller
        bool pulseDirection = true;

        // Start pulsing
        while (true)
        {
            if (pulseDirection)
            {
                // Increase size
                if (transform.localScale.x < maxPulseVector)
                {
                    transform.localScale += pulseIntensityVector;
                }
                else
                {
                    // Contract
                    pulseDirection = false;
                }
            }
            else
            {
                // Decrease size
                if (transform.localScale.x > minPulseVector)
                {
                    transform.localScale -= pulseIntensityVector;
                }
                else
                {
                    // Expand
                    pulseDirection = true;
                }
            }

            yield return new WaitForSeconds(fadeDelay * Time.deltaTime);
        }
    }

    public void StopFade()
    {
        if (fadeInRoutine != null)
        {
            StopCoroutine(fadeInRoutine);
        }
    }
}