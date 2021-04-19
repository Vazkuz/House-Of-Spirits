﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController gameController;
    public GameObject deckCanvas;
    [SerializeField] GameObject openDeckButton;
    public GameObject turnOptions;
    [SerializeField] GameObject cardOptions;
    public GameObject myCards;
    public GameObject cardsInGame;
    public GameObject cardChosen;
    public int currentTurn = 0;
    [SerializeField] int cardChosenIndex;
    public bool IveDrawnACard = false;
    public List<Card> cardsInGameList;

    void Awake()
    {
        if(gameController == null)
        {
            gameController = this;
        }
    }
    void Start()
    {
        deckCanvas.SetActive(false);
        cardOptions.SetActive(false);
        openDeckButton.SetActive(false);
        turnOptions.SetActive(false);
    }

    public void SetCardChosen(GameObject cardClicked)
    {
        cardChosen = cardClicked;
        cardChosenIndex = 0;
        foreach(Transform child in myCards.transform)
        {
            if (cardChosen != child.gameObject)
            {
                cardChosenIndex++;
            }
            else break;
        }
    }

    public void ViewDeck()
    {
        deckCanvas.SetActive(true);
        openDeckButton.SetActive(false);
        //turnOptions.SetActive(false);
    }

    public void CloseDeckOptions()
    {
        deckCanvas.SetActive(false);
        openDeckButton.SetActive(true);
        // foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        // {
        //     if (PhotonNetwork.PlayerList[GameController.gameController.currentTurn] == photonPlayer.GetComponent<PhotonView>().Owner &&
        //             photonPlayer.GetComponent<PhotonView>().IsMine)
        //     {
        //         turnOptions.SetActive(true);
        //     }
        // }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public void ToggleCardOptions(bool toogleValue)
    {
        if (cardOptions)
        {
            cardOptions.SetActive(toogleValue);
        }
        else
        {
            FindObjectOfType<GameController>().cardOptions.SetActive(toogleValue);
        }

        // Cuando apago las opciones de las cartas debo volver a "pintar" las cartas
        if(!toogleValue)
        {
            foreach(Transform child in myCards.transform)
            {
                child.gameObject.GetComponent<Image>().color = new Color(1,1,1);
            }
            cardChosenIndex = 0;
        }
    }

    public void PlayCardFromHand()
    {

        foreach(Transform child in myCards.transform)
        {
            child.gameObject.GetComponent<Image>().color = new Color(1,1,1);
        }

        foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            if(photonPlayer.GetComponent<PhotonView>().IsMine 
                    && photonPlayer.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[currentTurn])
            {                
                CloseDeckOptions();
                cardOptions.SetActive(false);
                for(int lookForPlayerIndex = 0; lookForPlayerIndex < PhotonNetwork.PlayerList.Length; lookForPlayerIndex++)
                {
                    if(photonPlayer.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[lookForPlayerIndex])
                    {
                        photonPlayer.LoseCardsFromHand();
                        photonPlayer.SendCardFromHandToTable(cardChosenIndex, lookForPlayerIndex);
                        GameController.gameController.currentTurn++;
                        if (GameController.gameController.currentTurn >= PhotonNetwork.PlayerList.Length)
                        {
                            GameController.gameController.currentTurn = 0;
                            Debug.Log("Updated current player to 0");
                        }
                        RoomController.room.PrepareSendingPlayerSequence(true);
                    }
                }
            }
        }

    }

    public void PassTurn()
    {
        foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            if (PhotonNetwork.PlayerList[GameController.gameController.currentTurn] == photonPlayer.GetComponent<PhotonView>().Owner &&
                    photonPlayer.GetComponent<PhotonView>().IsMine)
            {
                if(GameController.gameController.IveDrawnACard)
                {    
                    GameController.gameController.currentTurn++;
                    if (GameController.gameController.currentTurn >= PhotonNetwork.PlayerList.Length)
                    {
                        GameController.gameController.currentTurn = 0;
                        Debug.Log("Updated current player to 0");
                    }
                    RoomController.room.PrepareSendingPlayerSequence(true);
                    DrawSingleCard();
                    GameController.gameController.IveDrawnACard = false;
                }
            }
        }
    }

    public void DrawSingleCard()
    {
        foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            if (PhotonNetwork.PlayerList[GameController.gameController.currentTurn] == photonPlayer.GetComponent<PhotonView>().Owner &&
                    photonPlayer.GetComponent<PhotonView>().IsMine)
            {
                CardDisplay.cardDisplayInstance.DrawCards(1, photonPlayer, GameController.gameController.currentTurn);
            }
        }
        GameController.gameController.IveDrawnACard = true;
    }


}
