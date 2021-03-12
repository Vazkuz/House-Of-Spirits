using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject deckCanvas;
    [SerializeField] GameObject openDeckButton;

    void Start()
    {
        deckCanvas.SetActive(false);
        openDeckButton.SetActive(true);
    }
    public void ViewDeck()
    {
        deckCanvas.SetActive(true);
        openDeckButton.SetActive(false);
    }

    public void CloseDeckOptions()
    {
        deckCanvas.SetActive(false);
        openDeckButton.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.Disconnect();
    }
}
