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
    public int storyScene = 4;
    public int tutorialScene = 5;
    public bool firstTime = true;

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
