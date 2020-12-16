using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public LevelScreen lvlScreen;

    //! the load scene function will load the scene that matches the inputted scene
    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    //! the quit application function will quit the application
    public void QuitApplication()
    {
        // warn player about save etc etc
        Application.Quit();
    }
    public void ToggleLevelScreen(GameObject a)
    {
        a.SetActive(!a.activeSelf);
    }
}
