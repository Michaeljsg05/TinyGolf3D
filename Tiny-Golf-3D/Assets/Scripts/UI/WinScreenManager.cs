using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;


/*! 
 *  \brief     WinScreenManager Class
 *  \details   The Win Screen Manager Class will handle the win screen displayed after winning a level
 *  \author    Michael sansom
 *  \version   1.0b
 *  \date      20/11/2020
 */
public class WinScreenManager : MonoBehaviour
{
    public TMP_Text BestScore;
    public TMP_Text CurrentScore;

    //! LoadScene(string sceneName) is an overrided version of LoadScene that will load the scene that matches the input instead of working off the build index. can be helpful for levels that dont have another level after it.
    public void LoadScene(string sceneName)
    {
        // load the scene with matching name to input string

        LevelEvents.current.LevelEnd();
        StartCoroutine(Sleep(3, sceneName));
    }

    //! LoadScene() will attempt to load the next scene in the build settings aka load its own build index+1
    public void LoadScene()
    {
        LevelEvents.current.LevelEnd();
        StartCoroutine(Sleep(3));
    }

    public IEnumerator Sleep(float delay)
    {
        Debug.Log("Sleep started..");

        yield return new WaitForSeconds(delay);

        Scene t_thisScene = SceneManager.GetActiveScene();

        Debug.Log("It has been " + delay + " seconds!");
    }
    public IEnumerator Sleep(float delay, string name)
    {
        Debug.Log("Sleep started..");
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(name);
        Debug.Log("It has been " + delay + " seconds!");
    }

    //! ReloadScene() will attempt to reload the active scene
    public void ReloadScene()
    {
        try
        {
            LevelEvents.current.LevelEnd();
            Scene t_thisScene = SceneManager.GetActiveScene();
            StartCoroutine(Sleep(3, t_thisScene.name));
        }
        catch (System.Exception e)
        {
            Debug.Log("<color=red>Not working: " + e + "</color>");
        }
    }
}
