using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 30;
        Screen.SetResolution(1920, 1080, true);
    }

    public void PressPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void PressExit()
    {
        Application.Quit();
    }
}
