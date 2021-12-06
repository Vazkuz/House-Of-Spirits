using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class OptionHandler : MonoBehaviour
{
    public GameObject optionCanvas;
    [SerializeField] GameObject optionOpener;
    [SerializeField] GameObject turnOptions;
    [SerializeField] PUN2_Chat chatController;
    [SerializeField] GameObject secondBackground;
    
    void Start()
    {
        optionCanvas.SetActive(false);
        optionOpener.SetActive(true);
        if(secondBackground)
        {
            secondBackground.SetActive(false);
        }
    }
    
    public void OpenOptions()
    {
        optionCanvas.SetActive(true);
        optionOpener.SetActive(false);
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
        chatController.gameObject.SetActive(false);

        StateInitialAnimation();

    }

    void StateInitialAnimation()
    {
        foreach (PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            if(photonPlayer.PV.IsMine)
            {
                Debug.Log("The initial animation is: " + photonPlayer.mySelectedCharacter);
                FindObjectOfType<AvatarPreviewController>().ChangePreviewAnimation(photonPlayer.mySelectedCharacter);
                FindObjectOfType<AvatarPreviewController>().ChangePreviewGodsName(photonPlayer.mySelectedName);
            }
        }
    }

    public void CloseOptions()
    {
        optionCanvas.SetActive(false);
        optionOpener.SetActive(true);
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

        chatController.gameObject.SetActive(true);
    }
}
