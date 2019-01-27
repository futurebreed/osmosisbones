using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    [SerializeField]
    FMODUnity.StudioEventEmitter soundEmitter;

    private void Start()
    {
        soundEmitter.Play();
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
}