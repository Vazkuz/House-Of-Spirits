using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class OptionHandler : MonoBehaviour
{
    public GameObject optionCanvas;
    [SerializeField] GameObject optionOpener;
    [SerializeField] GameObject turnOptions;
    
    void Start()
    {
        optionCanvas.SetActive(false);
        optionOpener.SetActive(true);
    }
    
    public void OpenOptions()
    {
        optionCanvas.SetActive(true);
        optionOpener.SetActive(false);
        if (turnOptions)
        {
            turnOptions.SetActive(false);
        }
        foreach(GameObject nicknameShow in GameObject.FindGameObjectsWithTag("NicknameInput"))
        {
            nicknameShow.GetComponent<NicknameInputController>().enabled = true;
        }
        RoomController.room.DisableAvatarsTaken();
    }

    public void CloseOptions()
    {
        optionCanvas.SetActive(false);
        optionOpener.SetActive(true);
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
    }
}
