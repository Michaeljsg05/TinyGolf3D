using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*! 
 *  \brief    Pause Menu Class
 *  \details   The pause menu class will handle the pause menu in the main game loop, while the player is playing they can pause the game and that is when this class is active.
 *  \author    Michael sansom
 *  \version   1.0b
 *  \date      11/02/21
 */
public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseScreen = null;

    private bool isPaused = false;
    
    public bool canPause = true;

    private void Start()
    {
        PauseScreen.SetActive(false);
        isPaused = false;
    }


    private void Update()
    {
        // look for input
        if(Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            Pause();
        }

        
    }

    public void Toggle(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public void Pause()
    {
        // if escape is pressed toggle pause screen
        Toggle(PauseScreen);

        // toggle is paused
        isPaused = !isPaused;

        // toggle deltatime
        if (isPaused)
        {
            // slow down time
            Time.timeScale = 0.05f;
            GameObject.Find("LevelManager").GetComponent<LevelManager>().ball.GetComponent<BallController>().lockAim = true;

        }
        else
        {
            // speed up time
            Time.timeScale = 1;

            GameObject.Find("LevelManager").GetComponent<LevelManager>().ball.GetComponent<BallController>().lockAim = true;
        }
    }
}
