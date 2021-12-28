using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StoryController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] Sprite[] tutorialBackgrounds;
    [SerializeField] string[] tutorialStrings;
    Image imageComponent;
    int currentSprite;
    int totalSprites;

    void Start()
    {
        imageComponent = this.gameObject.GetComponent<Image>();
        currentSprite = 0;
        totalSprites = tutorialBackgrounds.Length;
        imageComponent.sprite = tutorialBackgrounds[currentSprite];
        tutorialText.text = tutorialStrings[currentSprite];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            currentSprite++;
            if(currentSprite >= totalSprites)
            {
                print("Go to Main Menu");
                SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.tutorialScene);
            }
            else
            {
                print("Change tutorial");
                imageComponent.sprite = tutorialBackgrounds[currentSprite];
                tutorialText.text = tutorialStrings[currentSprite];
            }
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            print("Go to Main Menu");
            SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
        }
    }
}
