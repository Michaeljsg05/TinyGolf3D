using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*! 
 *  \brief    Ball Manager Class
 *  \details   The ball manager class will handle the ball selection, for example the player can choose from a list of balls, that list will set the desired ball and the level manager will access this to spawn it
 *  \author    Michael sansom
 *  \version   1.0b
 *  \date      15/01/21
 */
public class BallManager : MonoBehaviour
{
    public static BallManager instance;

    [SerializeField]
    private GameObject ball = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public GameObject GetBall()
    {
        return ball;
    }

    public void SetBall(GameObject b)
    {
        ball = b;
    }

}
