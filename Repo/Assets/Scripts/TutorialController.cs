using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityObject = UnityEngine.Object;

public class TutorialController : MonoBehaviour
{    
    [SerializeField] TutorialElement[] tutorialBackgrounds;
    

    [Header("Light Controlling")]
    [SerializeField] Light2D pointLight;
    [SerializeField] Light2D globalLight;


    [Header("Screen Controlling")]
    [SerializeField] int currentBackground = 0;
    [SerializeField] int currentLightsIndex = 0;

    void Start()
    {
        SetBackgroundActive();

        if (!tutorialBackgrounds[currentBackground].lightController)
        {
            ToggleLighting(false);
        }
    }

    void SetBackgroundActive()
    {
        for (int backgroundIndex = 0; backgroundIndex < tutorialBackgrounds.Length; backgroundIndex++)
        {
            tutorialBackgrounds[backgroundIndex].gameObject.SetActive(false);
        }
        tutorialBackgrounds[currentBackground].gameObject.SetActive(true);
    }

    public void ChangeScreenButton()
    {
        if(!tutorialBackgrounds[currentBackground].lightController)
        {
            currentBackground++;
        }
        else
        {
            currentLightsIndex++;
        }

        if(currentBackground >= tutorialBackgrounds.Length)
        {
            print("Tutorial has ended.");
            SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
        }
        else
        {
            if(tutorialBackgrounds[currentBackground].lightController)
            {
                ToggleLighting(true);
                if(currentLightsIndex >= tutorialBackgrounds[currentBackground].lightPositions.Length)
                {
                    print("No more lights");
                }
                else
                {
                    ChangeScreenLights(tutorialBackgrounds[currentBackground], currentLightsIndex);
                    ChangeScreenScripts(tutorialBackgrounds[currentBackground], currentLightsIndex);
                }

                if(currentLightsIndex >= tutorialBackgrounds[currentBackground].lightPositions.Length)
                {
                    currentBackground++;
                    currentLightsIndex = 0;
                }

            }
            else
            {            
                ToggleLighting(false);
            }
        
            print("currentBackground: " + currentBackground);
            if(!tutorialBackgrounds[currentBackground].lightController)
            {
                print("Back to no lights");
                ToggleLighting(false);
            }
            else
            {
                ToggleLighting(true);
                ChangeScreenLights(tutorialBackgrounds[currentBackground], currentLightsIndex);
                ChangeScreenScripts(tutorialBackgrounds[currentBackground], currentLightsIndex);
            }
            
            if(currentBackground >= tutorialBackgrounds.Length)
            {
                print("Tutorial has ended.");
                QuitTutorial();
            }
            else
            {
                SetBackgroundActive();
            }
        }
    }

    void ChangeScreenLights(TutorialElement tutorialElement, int screenIndex)
    {
        print("New light position");
        pointLight.gameObject.transform.position = tutorialElement.lightPositions[screenIndex];
    }

    void ChangeScreenScripts(TutorialElement tutorialElement, int screenIndex)
    {
        tutorialElement.scriptText.text = tutorialElement.scripts[screenIndex].Replace("\\n", "\n");
        if(tutorialElement.scriptPosController)
        {
            tutorialElement.scriptText.transform.position = tutorialElement.scriptPositions[screenIndex];
        }
    }

    public void QuitTutorial()
    {
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
    }
    
    void ToggleLighting(bool lightsOn)
    {
        if(lightsOn)
        {
            print("Turn lights on");
            //Set up Point Light (intensity, radius, etc.)
            pointLight.intensity = tutorialBackgrounds[currentBackground].pointLightIntensity;
            pointLight.pointLightInnerRadius = tutorialBackgrounds[currentBackground].pointLightInnerRadius;
            pointLight.pointLightOuterRadius = tutorialBackgrounds[currentBackground].pointLightOuterRadius;

            //Set up Global Light
            globalLight.intensity = tutorialBackgrounds[currentBackground].globalLightIntensity;
        }
        else
        {
            print("Turn lights off");
            //Turn off Point Light
            pointLight.intensity = 0f;

            //Turn on Global Light
            globalLight.intensity = 1f;
        }
    }
}