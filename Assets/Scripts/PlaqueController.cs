using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaqueController : MonoBehaviour
{
    [SerializeField]
    private int startingLives = 3;

    private int lives;
    private Color startingColor;
    private Renderer plaqueRenderer;

    private void Awake()
    {
        lives = startingLives;
        plaqueRenderer = GetComponent<Renderer>();
        startingColor = plaqueRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            lives--;
            if (lives == 0)
            {
                lives = startingLives;
                plaqueRenderer.material.color = startingColor;
                transform.parent.gameObject.SetActive(false);
            }
            else
            {
                float percentageLeft = (float)lives / startingLives;
                float red = 1 - ((1 - startingColor.r) * percentageLeft);
                float green = startingColor.g * percentageLeft;
                float blue = startingColor.g * percentageLeft;
                Color color = new Color(red, green, blue);
                plaqueRenderer.material.color = color;
            }
        }
    }

    public void RestartValues()
    {
        lives = startingLives;
        plaqueRenderer.material.color = startingColor;
    }
}
