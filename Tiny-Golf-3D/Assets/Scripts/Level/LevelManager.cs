using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

/*! 
 *  \brief     Level Manager
 *  \details   The Level Manager Class will handle the level including ball and hole references, (add more later)
 *  \author    Michael sansom
 *  \version   1.0a
 *  \date      5/11/2020
 */
public class LevelManager : MonoBehaviour
{
    //! Gameobject hole is a reference to the hole object
    public GameObject hole;
    //! Gameobject ball is a reference to the ball object
    public GameObject ball;

    //! Int ShotCount is the amount of times the player has shot the ball
    public int ShotCount = -1;

    //! Int best score is the best score for this level
    public int BestScore = -1;

    //! bool New score set will be used to tell the player if a new best score was set
    bool newScoreSet;

    //! gameobject LevelWinScreen is a reference to the win screen. The player will be shown this after winning the level.
    [SerializeField]
    GameObject levelWinScreen = null;

    //! float level end transition time defines how long the client should wait before continuing with code. basically letting the transition finish.
    private float LevelEndTransitionTime = 3.0f;
    

    //! Start is called before the first frame update
    void Start()
    {
        hole = GameObject.Find("Hole");
        ball = GameObject.Find("Ball");
        LevelEvents.current.onLevelWin += OnWin;
        LevelEvents.current.onLevelEnd += LvlEnd;

        LevelEvents.current.onLevelLoad += LvlLoaded;
        LevelEvents.current.LevelLoad();


        if(levelWinScreen == null)
        {
            try
            {
                levelWinScreen = GameObject.Find("WinScreen");
            }
            catch(System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }

    }

    void OnWin()
    {
        // calculate if current score is better than best and set that

        // also set the best score

        // unlock the next level
        try
        {
            // make temp reference to save manager
            SaveManagement t_saveManager = GameObject.Find("SaveManagement").GetComponent<SaveManagement>();
            string[] t_levelInformation = t_saveManager.levels[SceneManager.GetActiveScene().buildIndex].Split(' ');
            if (t_levelInformation[6] != "none")
            {
                BestScore = int.Parse(t_levelInformation[6]);

                if (ShotCount < BestScore)
                {
                    // need to set best score:
                    newScoreSet = true;
                    t_levelInformation[6] = ShotCount.ToString();
                    string newInfo = t_levelInformation[0] + " " + t_levelInformation[1] + " " + t_levelInformation[2] + " " + t_levelInformation[3] + " " + t_levelInformation[4] + " " + t_levelInformation[5] + " " + t_levelInformation[6];
                    t_saveManager.levels[SceneManager.GetActiveScene().buildIndex] = newInfo;
                    t_saveManager.SaveLevels();
                    
                }
            }
            else
            {
                newScoreSet = true;
                t_levelInformation[6] = ShotCount.ToString();
                string newInfo = t_levelInformation[0] + " " + t_levelInformation[1] + " " + t_levelInformation[2] + " " + t_levelInformation[3] + " " + t_levelInformation[4] + " " + t_levelInformation[5] + " " + t_levelInformation[6];
                t_saveManager.levels[SceneManager.GetActiveScene().buildIndex] = newInfo;
                t_saveManager.SaveLevels();
            }
            if (newScoreSet)
            {
                BestScore = ShotCount;
                // play the best score animation/particals
            }
            // unlock next level
            t_saveManager.UnlockLevel((SceneManager.GetActiveScene().buildIndex +1).ToString());
        }
        catch (System.Exception e)
        {
            Debug.Log("<color=red>ERROR: " + e + "</color>");
        }

        // toggle the winScreen
        Debug.Log("Level Completed!");
        levelWinScreen.GetComponent<Animator>().SetTrigger("HasWon");

        // make a temporary reference to the win manager
        WinScreenManager t_winManager = levelWinScreen.GetComponent<WinScreenManager>();
        // set the text in the winmanager
        t_winManager.CurrentScore.text = "Current Score: " + ShotCount;
        t_winManager.BestScore.text = "Best Score: " + BestScore;




        // unsubscribe after the level has been won.
        LevelEvents.current.onLevelWin -= OnWin;
    }

    void LvlLoaded()
    {
        // show the starting transition
        Debug.Log("<color=green>Level has loaded successfully!</color>");



        LevelEvents.current.onLevelLoad -= LvlLoaded;
    }

    void LvlEnd()
    {
        // end the level
        GameObject.Find("Fader").GetComponent<Animator>().SetTrigger("FadeOut");
        LevelEvents.current.onLevelEnd -= LvlEnd;
    }
    
}
