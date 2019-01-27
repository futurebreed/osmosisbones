using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes : int
{
    MainMenu = 0,
    OsmosisScene
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
            SceneManager.LoadScene((int)Scenes.OsmosisScene);
        }
    }
}