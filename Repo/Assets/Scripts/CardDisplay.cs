﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject myCardsFolder;
    [SerializeField] float cardSpriteSize = 1.5f;
    [SerializeField] int maxPlayersJustOneDeck;
    public Vector3 cardLocalPosition;
    public float distanceBetweenCardsX = 360f;
    public float distanceBetweenCardsY = 300f;
    public int maxCardsPerRow = 7;
    [SerializeField] int cardsDrawnInitially = 8;
    public List<Card> cardsAvailable;
    // List<Card> myCards = new List<Card>();
    private PhotonView PV;

    // Instance
    public static CardDisplay cardDisplayInstance;
    public int indexCardOnLeft = 0;

    void Awake()
    {
        Debug.Log(7/2);
        if(cardDisplayInstance != null && cardDisplayInstance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            // Set the instance
            cardDisplayInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        cardDisplayInstance.cardsAvailable = new List<Card>(Resources.LoadAll<Card>("Cards")); //OJO: "Cards" es el path de donde se cargan todos los ScriptableObjects tipo Card (Resources)
        if (PhotonNetwork.CurrentRoom.PlayerCount > maxPlayersJustOneDeck)
        {
            cardsAvailable.AddRange(cardsAvailable);
        }
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(DealCards());
        }
    }

    IEnumerator DealCards()
    {
        yield return new WaitForSeconds(4);
        RoomController.room.PrepareSendingPlayerSequence(false, false, 0, 0);
        for(int playerIndex = 0; playerIndex < PhotonNetwork.PlayerList.Length; playerIndex++)
        {
            Debug.Log("Starting the draws: Player number " + playerIndex + ", player: " + PhotonNetwork.PlayerList[playerIndex]);
            foreach(PhotonPlayer playerCustom in FindObjectsOfType<PhotonPlayer>())
            {
                //Cuando encontremos al owner, lo usamos para instanciar todo
                if (PhotonNetwork.PlayerList[playerIndex] == playerCustom.GetComponent<PhotonView>().Owner)
                {
                    DrawCards(cardsDrawnInitially, playerCustom, playerIndex);
                }
            }
        }
        FirstCardInTable();
    }

    void FirstCardInTable()
    {
        int cardDrawnIndex = Random.Range(0, cardDisplayInstance.cardsAvailable.Count);
        PV.RPC("SendFirstCardInTableAllPlayers",RpcTarget.All,cardDrawnIndex);
        PV.RPC("RPC_RemoveFromDeck",RpcTarget.All,cardDrawnIndex);
    }

    

    [PunRPC]
    void SendFirstCardInTableAllPlayers(int cardChosenIndex)
    {
        if(cardDisplayInstance.cardsAvailable[cardChosenIndex].cardSuit == Card.CardSuit.Green)
        {
            GameController.gameController.gameBackground.sprite = GameController.gameController.greenBackgroud;
        }
        else if(cardDisplayInstance.cardsAvailable[cardChosenIndex].cardSuit == Card.CardSuit.Orange)
        {
            GameController.gameController.gameBackground.sprite = GameController.gameController.orangeBackgroud;
        }
        else if(cardDisplayInstance.cardsAvailable[cardChosenIndex].cardSuit == Card.CardSuit.Purple)
        {
            GameController.gameController.gameBackground.sprite = GameController.gameController.purpleBackgroud;
        }
        else if(cardDisplayInstance.cardsAvailable[cardChosenIndex].cardSuit == Card.CardSuit.Yellow)
        {
            GameController.gameController.gameBackground.sprite = GameController.gameController.yellowBackgroud;
        }

        GameObject cardPlayed = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        cardPlayed.GetComponent<HoverButton>().enabled = false;
        cardPlayed.transform.SetParent(GameController.gameController.cardsInGame.transform, false);
        cardPlayed.name = cardDisplayInstance.cardsAvailable[cardChosenIndex].cardNumber.ToString() + " " 
                            + cardDisplayInstance.cardsAvailable[cardChosenIndex].cardSuit.ToString();

        cardPlayed.transform.localScale = new Vector3(cardSpriteSize, cardSpriteSize, 0);
        cardPlayed.transform.localScale = new Vector3(1f, 1f, 0);
        cardPlayed.GetComponent<Button>().enabled = false;
                    
        // We configure it's number and suit. We'll use that info later for the game mechanics.
        cardPlayed.GetComponent<CardController>().SetCardSuit(cardDisplayInstance.cardsAvailable[cardChosenIndex].cardSuit);
        cardPlayed.GetComponent<CardController>().SetCardNumber(cardDisplayInstance.cardsAvailable[cardChosenIndex].cardNumber);

        cardPlayed.GetComponent<Image>().overrideSprite = cardDisplayInstance.cardsAvailable[cardChosenIndex].artwork;

        GameController.gameController.cardsInGameList.Add(cardDisplayInstance.cardsAvailable[cardChosenIndex]);

    }

    public void DrawCards(int cardsToDrawn, PhotonPlayer photonPlayer, int playerIndex)
    {
        if(cardDisplayInstance.cardsAvailable.Count == 0)
        {
            PV.RPC("MixCardsInGame",RpcTarget.All);
        }

        for (int drawIndex = 0; drawIndex < cardsToDrawn; drawIndex++)
        {
            int cardDrawnIndex = Random.Range(0, cardDisplayInstance.cardsAvailable.Count);
            PV.RPC("RPC_AddToHand",RpcTarget.All,cardDrawnIndex, playerIndex);
            PV.RPC("SetupDrawnCard", RpcTarget.All,playerIndex, drawIndex);
            PV.RPC("RPC_RemoveFromDeck",RpcTarget.All,cardDrawnIndex);
        }
    }

    [PunRPC]
    void MixCardsInGame()
    {
        int cardsInGameAtMomentOfDraw = GameController.gameController.cardsInGameList.Count;
        for (int cardInGameIndex = 0; cardInGameIndex < cardsInGameAtMomentOfDraw - 1; cardInGameIndex++)
        {
            cardDisplayInstance.cardsAvailable.Add(GameController.gameController.cardsInGameList[cardInGameIndex]);
        }

        for (int cardInGameIndex = 0; cardInGameIndex < cardsInGameAtMomentOfDraw - 1; cardInGameIndex++)
        {
            GameController.gameController.cardsInGameList.RemoveAt(0);
            Destroy(GameController.gameController.cardsInGame.transform.GetChild(0).gameObject);
        }
    }

    [PunRPC]
    void RPC_AddToHand(int cardDrawnIndex, int playerIndex)
    {
        foreach(PhotonPlayer playerCustom in FindObjectsOfType<PhotonPlayer>())
        {
            if (PhotonNetwork.PlayerList[playerIndex] == playerCustom.GetComponent<PhotonView>().Owner)
            {
                playerCustom.myCards.Add(cardDisplayInstance.cardsAvailable[cardDrawnIndex]);
                if(playerCustom.GetComponent<PhotonView>().IsMine)
                {
                    playerCustom.AddCardToHand();
                    playerCustom.UpdateNumberOfCardsInDisplay(playerIndex, playerCustom.GetNumberOfCards().ToString());
                }
            }
        }
    }

    [PunRPC]
    void RPC_RemoveFromDeck(int cardDrawnIndex)
    {
        // Debug.Log("Card to remove: " + instance.cardsAvailable[cardDrawnIndex]);
        cardDisplayInstance.cardsAvailable.Remove(cardDisplayInstance.cardsAvailable[cardDrawnIndex]);
    }

    [PunRPC]
    void SetupDrawnCard(int playerIndex, int drawIndex)
    {
        foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            //Cuando encontremos al owner, lo usamos para instanciar todo
            if (PhotonNetwork.PlayerList[playerIndex] == photonPlayer.GetComponent<PhotonView>().Owner && photonPlayer.GetComponent<PhotonView>().IsMine)
            {
                // Name and parenting of the card drawn. We parent it in a GO/folder to organize them easily. 
                // GameObject cardDrawn = new GameObject(myCards[drawIndex].cardNumber.ToString() + " " + myCards[drawIndex].cardSuit.ToString());
                GameObject cardDrawn = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                cardDrawn.transform.SetParent(myCardsFolder.transform, false);
                cardDrawn.name = photonPlayer.myCards[photonPlayer.myCards.Count - 1].cardNumber.ToString() + " "
                                    + photonPlayer.myCards[photonPlayer.myCards.Count - 1].cardSuit.ToString();

                cardDrawn.transform.localScale = new Vector3(cardSpriteSize, cardSpriteSize, 0);

                // We configure it's number and suit. We'll use that info later for the game mechanics.
                cardDrawn.GetComponent<CardController>().SetCardSuit(photonPlayer.myCards[photonPlayer.myCards.Count - 1].cardSuit);
                cardDrawn.GetComponent<CardController>().SetCardNumber(photonPlayer.myCards[photonPlayer.myCards.Count - 1].cardNumber);

                // Setup the visuals
                cardDrawn.GetComponent<Image>().sprite = photonPlayer.myCards[photonPlayer.myCards.Count - 1].artwork;
                ArrangeCards(drawIndex, cardDrawn);
            }
        }
    }

    private void ArrangeCards(int drawIndex, GameObject cardDrawn)
    {
        for(int playerIndex = 0; playerIndex < PhotonNetwork.PlayerList.Length; playerIndex++)
        {
            foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
            {
                //Cuando encontremos al owner, lo usamos para instanciar todo
                if (PhotonNetwork.PlayerList[playerIndex] == photonPlayer.GetComponent<PhotonView>().Owner && photonPlayer.GetComponent<PhotonView>().IsMine)
                {
                    if (photonPlayer.cardsIHave <= maxCardsPerRow)
                    {
                        cardDrawn.transform.localPosition = new Vector3((maxCardsPerRow / 2 - drawIndex) * distanceBetweenCardsX, /*- distanceBetweenCardsY * multiplyBy*/ 0, 0);
                    }
                    else
                    {
                        cardDrawn.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
