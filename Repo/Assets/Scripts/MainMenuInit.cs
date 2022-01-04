using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuInit : MonoBehaviour
{
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject mainBackground;
    [SerializeField] GameObject logo;
    [SerializeField] GameObject spirits;
    [SerializeField] GameObject connectingToServersText;
    Animator backgroundAnimator;
    Animator logoAnimator;
    Animator spiritsAnimator;
    
    void Start()
    {
        backgroundAnimator = this.gameObject.GetComponent<Animator>();
        logoAnimator = logo.GetComponent<Animator>();
        spiritsAnimator = spirits.GetComponent<Animator>();
        if(!MultiplayerSettings.multiplayerSettings.firstTime)
        {
            backgroundAnimator.SetBool("FirstTime", false);
            logoAnimator.SetBool("FirstTime", false);
            spiritsAnimator.SetBool("FirstTime", false);
        }
    }
    
    public void StartInitializationBackground()
    {
        print("Lets first show background");
        backgroundAnimator.SetBool("FadeIn", true);
        buttons.SetActive(false);
        logo.SetActive(false);
        spirits.SetActive(false);
        connectingToServersText.SetActive(false);
    }

    public void FinishBackgroundFadeIn()
    {
        backgroundAnimator.SetBool("FadeIn", false);
    }

    public void ShowEverything()
    {
        if(PhotonNetwork.IsConnected)
        {
            buttons.SetActive(true);
        }
        else
        {
            connectingToServersText.SetActive(true);
        }
    }

    public void StartLogoFadeIn()
    {
        logo.SetActive(true);
        logoAnimator.SetBool("FadeIn", true);
    }    

    public void StartSpiritsFadeIn()
    {
        spirits.SetActive(true);
        spiritsAnimator.SetBool("FadeIn", true);
    }
}
