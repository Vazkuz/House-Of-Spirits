using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents gameEvents;

    void Awake()
    {
        gameEvents = this;
    }
    
    public event Action onObjectEnabled;
    public void ObjectEnabled()
    {
        if (onObjectEnabled != null)
        {
            onObjectEnabled();
        }
    }

}
