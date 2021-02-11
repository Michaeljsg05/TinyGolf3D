using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*! 
 *  \brief    Lock View Class
 *  \details   The lock view class will handle locking the view so it cannot be rotated while playing the game
 *  \author    Michael sansom
 *  \version   1.0b
 *  \date      20/12/20
 */

public class LockView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape; 
    }
}
