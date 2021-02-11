using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*! 
 *  \brief     Performance manager class
 *  \details   The performance manager class will handle the performance settings for the games.
 *  \author    Michael sansom
 *  \version   1.1b
 *  \date      5/1/21
 */
public class PerformanceManager : MonoBehaviour
{

    // manage performance modes
    public enum PerformanceModes
    {
        low,
        high
    }

    public PerformanceModes currentMode;


    [Header("Water Settings")]

    [SerializeField]
    private Material lowWater;
    [SerializeField]
    private Material highWater;




    // Start is called before the first frame update
    void Start()
    {
        // try get the water and set it to the apropriate water texture
        MeshRenderer t_waterRenderer = GameObject.Find("Water").GetComponent<MeshRenderer>();
        if(t_waterRenderer != null)
        {
            switch (currentMode)
            {
                case PerformanceModes.low:
                    // low mode: Set water to the low material (Flat)
                    t_waterRenderer.material = lowWater;
                    break;
                case PerformanceModes.high:
                    // high mode: set water to the high material (Shader)
                    t_waterRenderer.material = highWater;
                    break;
            }

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
