using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes : int
{
    MainMenu = 0,
    GameScene
}

public class MenuController : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        bool aButtonPressed = Input.GetButtonUp("Fire1");
        if (aButtonPressed)
        {
            SceneManager.LoadScene((int)Scenes.GameScene);
        }
    }
}