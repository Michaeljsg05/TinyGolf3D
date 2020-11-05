using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    

    //! Start is called before the first frame update
    void Start()
    {
        hole = GameObject.Find("Hole");
        ball = GameObject.Find("Ball");
    }

    //! Update is called once per frame
    void Update()
    {
        
    }
}
