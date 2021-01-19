using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*! 
 *  \brief     Camera Manager
 *  \details   The Camera Manager Class will take care of the cameras. it will make sure the ball and hole are always in frame and keep the overlay renderer aligned with the game renderer.
 *  \author    Michael sansom
 *  \version   2.5a
 *  \date      4/11/2020
 */
public class CameraManager : MonoBehaviour
{
    /*! \brief LevelManager level manager stores a reference to the levels manager.
    * 
    *  This level manager is used to gain access to the hole and ball object in this instance.
    */
    LevelManager levelManager;

    /*! \brief Camera main level camera stores a reference to the main level camera.
    * 
    *  this camera is used to affect the players view.
    */
    public Camera mainLevelCamera;

    /*! \brief Camera overlay camera stores a reference to the overlay camera.
    * 
    *  This camera is used to lock alignment with the playercamera and keep overlaying images such as ui drawn on top of the game.
    */
    public Camera overlayCamera;

    /*! \brief float minimum distance is the minimum distance relative to camera sizing (so it doesnt get too close).
     * 
     * The minimum distance is checked so if the ball is further than the minimum distance the manager will start to zoom out a little bit.
     */
    public float minDistance = 5;


    /*! \brief float original size is the original orthographic size of the main camera.
    * 
    * The original size is the original orthographic size or fov of the original camera. this original size
    * will be used as a reference when zooming back in after the player returns to the radius of the min zoom distance.
    */
    float originalSize;

    /*! \brief float zoom smooth is the smoothing time factor.
    * 
    * the zoom smooth variable is the smoothing time factor or sensitivity, this will affect how fast or slow the zooming and moving will go.
    */
    public float zoomSmooth = 1.5f;

    /*! \brief float Zoom Offset is the amount the zoom will be changed by the player input
    * 
    * the zoom offset will be how much the zoom is offset when the player zooms in or out
    */
    public float zoomOffset = 5.0f;

    /*! \brief float Zoom Offset Min is the min amount the zoom can be changed by the player input
    * 
    * The zoom offset min is the minimum amount the zoom can be affected by player zooming in and out
    */
    public float zoomOffsetMin = 3.0f;

    /*! \brief float Zoom Offset Max is the Max amount the zoom can be changed by the player input
    * 
    * The zoom offset max is the maximum amount the zoom can be affected by player zooming in and out
    */
    public float zoomOffsetMax = 10.0f;


    /*! \brief float aspect offset will be how much the camera zooms in or out by default depending on the screen size
    * 
    * the aspect offset will be how much the camera zooms in or out by default depending on the screensize of the device that the user is on.
    */
    public float aspectOffset;

    /*! \brief Vector3 original position is the original position of the camera pivot
     * the original postion vector will store the beginning position of the camera pivot. this will be used to lock the Y axis of the
     * position transform.
     */
    Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        // get the level manager
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        originalSize = mainLevelCamera.orthographicSize;
        originalPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the cameras are not null
        if(overlayCamera != null && mainLevelCamera != null)
        {
            // lock the camera sizes so there is no bad allignment
            overlayCamera.orthographicSize = mainLevelCamera.orthographicSize;
        }
        //var t_aspect = mainLevelCamera.aspect;
        //
        //if(t_aspect < 0.6)
        //{
        //    // most likley a mobile device so zoom out
        //    aspectOffset = 3;
        //    Debug.Log("Mobile");
        //    // zoom out alot
        //}
        //else if(t_aspect >0.6 && t_aspect < 1.5f)
        //{
        //    // most likley a square monitor (rounded up)
        //    aspectOffset = 0.5f;
        //    Debug.Log("squareScreen");
        //    // zoom out a little bit
        //}
        //else if(t_aspect > 1.5f && t_aspect < 2.5f)
        //{
        //    // widescreen monitor
        //    aspectOffset = 0;
        //    Debug.Log("widescreen");
        //    // leave the zoom
        //}
        //else if(t_aspect > 2.5f)
        //{
        //    // a ultra wider monitor
        //    aspectOffset = -0.2f;
        //    Debug.Log("ultrawide");
        //    // zoom in a little for better visibility
        //}

        // code to zoom in and out

        // pc - Scroll to zoom
        zoomOffset += (Input.mouseScrollDelta.y / 5);
        
        // mobile - Pinch to zoom

        if(Input.touchCount == 2)
        {
            // if there are 2 touches at one time
            
            Touch touchOne = Input.GetTouch(0);
            Touch touchTwo = Input.GetTouch(1);
            
            Vector2 TouchOnePrevPosition = touchOne.position - touchOne.deltaPosition;
            Vector3 TouchTwoPrevPosition = touchTwo.position - touchTwo.deltaPosition;
            
            float prevDist = (TouchOnePrevPosition - TouchOnePrevPosition).magnitude;
            float currentDist = (touchOne.position - touchTwo.position).magnitude;

            float difference = currentDist - prevDist;

            Debug.Log("difference is: " + difference); // DEBUG 
        }


        // clamp the zoom offset value to min and max offset values
        if(zoomOffset < zoomOffsetMin)
        {
            zoomOffset = zoomOffsetMin;
        }
        if(zoomOffset > zoomOffsetMax)
        {
            zoomOffset = zoomOffsetMax;
        }


        // check if the level manager has found the hole and ball
        if(levelManager.hole != null && levelManager.ball != null)
        {
            // if the hole and ball could be found calculate the distance from the ball to the hole
            float t_distance = Vector3.Distance(levelManager.hole.transform.position, levelManager.ball.transform.position);

            // check if distance is greater than the minimum distance
            if(t_distance > minDistance)
            {
                // if distance > min distance then change the size
                // sizing the camera:
                float t_desiredSize = originalSize + (t_distance / 10);
                // lerp the main camera size to itself plus distance / 5
                mainLevelCamera.orthographicSize = Mathf.Lerp(mainLevelCamera.orthographicSize, t_desiredSize + zoomOffset + aspectOffset, Time.deltaTime * zoomSmooth);

                // moving the camera:
                Vector3 t_desPosition = (levelManager.hole.transform.position + levelManager.ball.transform.position) / 2;
                transform.position = Vector3.Lerp(transform.position, t_desPosition, Time.deltaTime * zoomSmooth);
                transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
                
            }
            else
            {
                // if inside distance zoom back into original zoom
                mainLevelCamera.orthographicSize = Mathf.Lerp(mainLevelCamera.orthographicSize, originalSize + zoomOffset, Time.deltaTime * zoomSmooth);
                transform.position = Vector3.Lerp(transform.position, levelManager.hole.transform.position, Time.deltaTime / 2);
            }
        }
    }
}
