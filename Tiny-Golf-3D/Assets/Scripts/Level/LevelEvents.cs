using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvents : MonoBehaviour
{
    public static LevelEvents current;
    private void Awake()
    {
        current = this;
    }

    public event Action onLevelWin;

    public void LevelWin()
    {
        if(onLevelWin != null)
        {
            onLevelWin();
        }
    }

    public event Action onLevelLoad;

    public void LevelLoad()
    {
        if(onLevelLoad != null)
        {
            onLevelLoad();
        }
    }

    public event Action onLevelEnd;
    public void LevelEnd()
    {
        if(onLevelEnd != null)
        {
            onLevelEnd();
        }
    }
}
