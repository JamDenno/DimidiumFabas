using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void PlayButton()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void ControlsButton()
    {
        SceneManager.LoadScene("ControlsScene", LoadSceneMode.Single);
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
