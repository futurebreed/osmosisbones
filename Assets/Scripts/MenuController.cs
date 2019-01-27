using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes : int
{
    MainMenu = 0,
    GameScene,
    Credits
}

public class MenuController : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        audioManager.PlayBackgroundAudio(Guid.Parse("92ea6681-03a1-42f3-b016-9c14447678d2"), gameObject);
    }

    private void Update()
    {
        bool aButtonPressed = Input.GetButtonUp("Fire1");
        if (aButtonPressed)
        {
            SceneManager.LoadScene((int)Scenes.GameScene);
            audioManager.Stop();
        }

        bool quitButtonPressed = Input.GetButtonUp("Quit");
        if (quitButtonPressed)
        {
            Debug.Log("Quit game");
            Application.Quit();
        }
    }
}