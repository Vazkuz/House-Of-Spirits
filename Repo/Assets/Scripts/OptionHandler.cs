using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionHandler : MonoBehaviour
{
    public GameObject optionCanvas;
    [SerializeField] GameObject turnOptions;
    [SerializeField] PUN2_Chat chatController;
    [SerializeField] GameObject secondBackground;
    
    void Start()
    {
        optionCanvas.SetActive(false);
        if(secondBackground)
        {
            secondBackground.SetActive(false);
        }
    }
    
    public void OpenOptions()
    {
        optionCanvas.SetActive(true);
        if(secondBackground)
        {
            secondBackground.SetActive(true);
        }
        if (turnOptions)
        {
            turnOptions.SetActive(false);
        }

        foreach(GameObject nicknameShow in GameObject.FindGameObjectsWithTag("NicknameInput"))
        {
            nicknameShow.GetComponent<NicknameInputController>().enabled = true;
        }

        RoomController.room.DisableAvatarsTaken();
        if(chatController)
        {
            chatController.gameObject.SetActive(false);
        }

        if(SceneManager.GetActiveScene().buildIndex == MultiplayerSettings.multiplayerSettings.roomScene)
        {
            StateInitialAnimation();
            AvatarPreviewController.APC.previousSelection = AvatarPreviewController.APC.currentAnimation;
        }

    }

    void StateInitialAnimation()
    {
        foreach (PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            if(photonPlayer.PV.IsMine)
            {
                Debug.Log("The initial animation is: " + photonPlayer.mySelectedCharacter);
                FindObjectOfType<AvatarPreviewController>().ChangePreviewAnimation(photonPlayer.mySelectedCharacter);
                // FindObjectOfType<AvatarPreviewController>().ChangePreviewGodsName(photonPlayer.mySelectedCharacter);
            }
        }
    }

    public void CloseOptions()
    {
        optionCanvas.SetActive(false);
        if(secondBackground)
        {
            secondBackground.SetActive(false);
        }
        if (turnOptions)
        {
            foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
            {
                if (PhotonNetwork.PlayerList[GameController.gameController.currentTurn] == photonPlayer.GetComponent<PhotonView>().Owner &&
                        photonPlayer.GetComponent<PhotonView>().IsMine)
                {
                    turnOptions.SetActive(true);
                }
            }
        }

        foreach(GameObject nicknameShow in GameObject.FindGameObjectsWithTag("NicknameInput"))
        {
            nicknameShow.GetComponent<NicknameInputController>().enabled = false;
        }

        if(chatController)
        {
            chatController.gameObject.SetActive(true);
        }
    }
}
