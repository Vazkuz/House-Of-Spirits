using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] Sprite[] tutorialBackgrounds;
    Image imageComponent;
    int currentSprite;
    int totalSprites;

    void Start()
    {
        imageComponent = this.gameObject.GetComponent<Image>();
        currentSprite = 0;
        totalSprites = tutorialBackgrounds.Length;
        imageComponent.sprite = tutorialBackgrounds[currentSprite];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            currentSprite++;
            if(currentSprite >= totalSprites)
            {
                print("Go to Main Menu");
                SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
            }
            else
            {
                print("Change tutorial");
                imageComponent.sprite = tutorialBackgrounds[currentSprite];
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("Go to Main Menu");
            SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
        }
    }
}
