using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes : int
{
    MainMenu = 0,
    GameScene
}

public class MenuController : MonoBehaviour
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
        if (aButtonPressed)
        {
            SceneManager.LoadScene((int)Scenes.GameScene);
            soundEmitter.Stop();
        }

        bool quitButtonPressed = Input.GetButtonUp("Quit");
        if (quitButtonPressed)
        {
            Debug.Log("Quit game");
            Application.Quit();
        }
    }
}