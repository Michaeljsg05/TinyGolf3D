using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/*! 
 *  \brief     Ball Controller Class
 *  \details   The ball controller class handles all player/ball movement and input.
 *  \author    Michael sansom
 *  \version   1.3b
 *  \date      5/11/2020
 */
public class BallController : MonoBehaviour
{

    //! max distance the player can move their finger from the ball (for chargeup)
    public float maxDistance = 3;

    //! float minDistance is the minimum distance the player can drag from the ball before the shot is cancelled
    public float minDistance = 1.5f;

    //! ball speed
    public float speed = 4;

    //! max speed the ball can be going to enter hole
    public float maxHoleEnterSpeed = 3;

    //! the current move speed
    float currentMoveSpeed;

    //! bool stores if the player is aiming
    bool isAiming;
    //! bool stores if the player is moving
    bool isMoving;
    //! bool stores if the player can aim
    bool canAim;

    //! bool stores aim lock (for pausing)
    public bool lockAim = false;

    //! bool stores if the player is in the hole
    bool inHole;

    //! vector3 that stores the position of the hole
    Vector3 holePos;
    //! vector3 that stores the start position of the line (drag)
    Vector3 startPos;
    //! vectoe3 that stores the end position of the line (drag)
    Vector3 endPos;

    //! line render that stores a reference to the line renderer so the player can see where they are aiming
    LineRenderer line;

    //! Rigidbody component that stores the rigidbody on that ball
    Rigidbody rb;

    //! shot count will keep track of the shots then will be passed into the correct manager when the level is completed
    int shotCount = 0;

    //! bool load complete will store if the level has completed loading
    bool loadComplete = false;

    //! bool aimtooclose will be set based off min distance and will say if the aim is too close to the ball
    bool aimTooClose;

    //! shotcounttext is the text that shows the player the shot count
    TMP_Text shotcounttext;

    //! vector that stores initial tap position
    Vector3 initTapPos;

    //! Start called at the start of the scene
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        line = GameObject.Find("LineObject").GetComponent<LineRenderer>();
        StartCoroutine(DisableShootingForSeconds(1));
        shotcounttext = GameObject.Find("ShotCounter").GetComponent<TMP_Text>();
    }

    //! disable shooting for seconds (time) will stop the player from shooting for a certain amount of time
    public IEnumerator DisableShootingForSeconds(float time)
    {
        Debug.Log("waiting for " + time + "seconds");
        loadComplete = false;
        yield return new WaitForSeconds(time);
        loadComplete = true;
        Debug.Log("done waiting");
    }

    //! Update called once per frame
    void Update()
    {

        if (rb.velocity.magnitude <= 0.1f && !lockAim) // if the ball has stopped (0.1f instead of 0 because it will never be exactually at 0)
        {
            canAim = true;
        }
        else
        {
            canAim = false;
        }

        if (Input.GetMouseButtonDown(0) && !isAiming && canAim && !inHole && loadComplete)
        {
            // if the player taps / clicks
            isAiming = true;
            canAim = false;

            // set init tap pos to mouse pos
            initTapPos = Input.mousePosition;


        }
        if (isAiming)
        {
            // while we are aiming we want to:

            // enable line renderer
            // do later

            // set the start pos to where our ball is
            startPos = this.transform.position;

            // create a vector that can be used to calculate directions
            Vector3 t_newPos = new Vector3();

            // raycast from the camera through the mouse position, cant use camera.screenToWorldPoint because of 'complex rotations'
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit camHit;
            if (Physics.Raycast(camRay, out camHit, 100))
            {
                if (camHit.transform.gameObject.layer == 4)
                    t_newPos = camHit.point;
                // adding a layermask breaks it..
            }
            t_newPos.y = 0.05f;// offset the new positions y level a little bit so things are more visible

            // invert t_newpos to be infront not behind
            t_newPos = transform.position + (transform.position - t_newPos);

            // set endpos to new pos
            endPos = t_newPos;

            // clamp endpos to maxDistance
            if (Vector3.Distance(startPos, t_newPos) > maxDistance)
            {
                Vector3 dir = endPos - startPos;
                endPos = this.transform.position + (dir.normalized * maxDistance);
            }

            // Check if distance is too close, if it is then cancel
            if (Vector3.Distance(startPos, t_newPos) < minDistance)
            {
                aimTooClose = true;
            }
            else
            {
                aimTooClose = false;
            }


            // line renderer!

            line.enabled = true;
            line.SetPosition(0, startPos);
            line.SetPosition(1, endPos);
        }
        else
        {
            // turn line renderer off
            line.enabled = false;
        }

        if (Input.GetMouseButtonUp(0) && isAiming && !lockAim)
        {
            // if we let go of mouse while aiming set can aim to true, aiming to false and shoot!

            // check if the position is the same as initital position
            if (Input.mousePosition != initTapPos && !aimTooClose)
                Shoot();

            isAiming = false;
            canAim = true;
        }

        if (lockAim)
        {
            isAiming = false;
            canAim = false;
        }

        if (inHole)
        {
            // make sure the pause menu cannot be opened
            GameObject.Find("PauseMenu").GetComponent<PauseMenu>().canPause = false;
            Debug.Log("Pause triggered");
            Vector3 t_desPos = new Vector3(holePos.x, holePos.y - 1, holePos.z);
            transform.position = Vector3.Lerp(this.transform.position, t_desPos, Time.deltaTime);
            Destroy(this.gameObject, 1.0f);
            GameObject.Find("LevelManager").GetComponent<LevelManager>().ShotCount = shotCount;
            Debug.Log("Level over");
            // trigger level win.
            LevelEvents.current.LevelWin();
        }

    }


    /**
     * @title Shoot mechanic
     * @brief Shoot the ball in the direction of the charge up (specified by player input)
     */
    private void Shoot()
    {
        Vector3 dir = (endPos - startPos);
        Debug.Log("Shoot!");
        shotCount++;
        shotcounttext.text = shotCount.ToString();
        rb.AddForce(dir * speed);
    }

    //! OnTriggerEnter being used when the ball enters a hole
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hole" && rb.velocity.magnitude < maxHoleEnterSpeed)
        {
            Debug.Log(gameObject.name + " has entered " + other.gameObject.name);
            inHole = true;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            holePos = other.transform.position;
        }
        if (other.tag == "SpeedPad")
        {
            Debug.Log("Hit Speedpad");
            rb.velocity *= 2;
        }
        if (other.tag == "JumpPad")
        {
            Debug.Log("Hit JumpPad");
            rb.AddForce(new Vector3(0, 30, 0), ForceMode.Impulse);
        }
    }

}
