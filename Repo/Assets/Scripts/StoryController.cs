using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class StoryController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] TextMeshProUGUI tutorialShadowText;
    [SerializeField] Sprite[] tutorialBackgrounds;
    [SerializeField] string[] tutorialStrings;
    [SerializeField] int[] avoidFade;
    Image imageComponent;
    int currentSprite;
    int totalSprites;
    bool firstTime = true;
    Animator animator;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        imageComponent = this.gameObject.GetComponent<Image>();
        currentSprite = 0;
        totalSprites = tutorialBackgrounds.Length;
        imageComponent.sprite = tutorialBackgrounds[currentSprite];
        tutorialText.text = tutorialStrings[currentSprite].Replace("\\n", "\n");
        tutorialShadowText.text = tutorialStrings[currentSprite].Replace("\\n", "\n");
        animator.SetInteger("Fade", 1);
    }

    public void ChangeStoryIlustration()
    {
        currentSprite++;
        tutorialText.text = null;
        tutorialShadowText.text = null;
        if(!ArrayUtility.Contains(avoidFade, currentSprite))
        {
            animator.SetInteger("Fade", -1);
        }
        else
        {
            ChangeStoryAfterAnimation();
        }
    }

    public void ChangeStoryAfterAnimation()
    {
        if(!firstTime)
        {
            if (currentSprite >= totalSprites)
            {
                print("Go to Main Menu");
                SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.tutorialScene);
            }
            else
            {
                print("Change tutorial");
                imageComponent.sprite = tutorialBackgrounds[currentSprite];
                tutorialText.text = tutorialStrings[currentSprite].Replace("\\n", "\n");
                tutorialShadowText.text = tutorialStrings[currentSprite].Replace("\\n", "\n");
            }
        }
        animator.SetInteger("Fade", 0);
    }

    public void StopFadeIn()
    {
        if(firstTime)
        {
            animator.SetInteger("Fade", 0);
            firstTime = false;
        }
    }

    public void QuitTutorial()
    {
        print("Go to Main Menu");
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
    }

}
