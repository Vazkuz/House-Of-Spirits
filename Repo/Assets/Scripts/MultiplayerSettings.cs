using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerSettings : MonoBehaviour
{
    public static MultiplayerSettings multiplayerSettings;
    public int maxPlayers = 6;
    public int menuScene;
    public int roomScene;
    public int gameScene;
    public int gameEndedScene;

    void Awake()
    {
        if (MultiplayerSettings.multiplayerSettings == null)
        {
            MultiplayerSettings.multiplayerSettings = this;
        }
        else 
        {
            if (MultiplayerSettings.multiplayerSettings != this) Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
