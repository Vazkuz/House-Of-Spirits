﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] GameObject myDeckButton;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject myCardsFolder;
    [SerializeField] float cardSpriteSize = 1.5f;
    [SerializeField] Vector3 cardLocalPosition;
    [SerializeField] float distanceBetweenCardsX = 200f;
    [SerializeField] float distanceBetweenCardsY = 300f;
    [SerializeField] int maxCardsPerRow = 8;
    [SerializeField] int cardsDrawnInitially = 8;
    public List<Card> cardsAvailable;
    // List<Card> myCards = new List<Card>();
    private PhotonView PV;

    // Instance
    public static CardDisplay instance;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            // Set the instance
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        instance.cardsAvailable = new List<Card>(Resources.LoadAll<Card>("Cards")); //OJO: "Cards" es el path de donde se cargan todos los ScriptableObjects tipo Card (Resources)
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(DealCards());
        }
    }

    IEnumerator DealCards()
    {
        yield return new WaitForSeconds(4);
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
    }

    void DrawCards(int cardsToDrawn, PhotonPlayer photonPlayer, int playerIndex)
    {
        for (int drawIndex = 0; drawIndex < cardsToDrawn; drawIndex++)
        {
            int cardDrawnIndex = Random.Range(0, instance.cardsAvailable.Count);
            PV.RPC("RPC_AddToHand",RpcTarget.All,cardDrawnIndex, playerIndex);
            PV.RPC("SetupDrawnCard", RpcTarget.All,playerIndex);
            PV.RPC("RPC_RemoveFromDeck",RpcTarget.All,cardDrawnIndex);
        }
    }

    [PunRPC]
    void RPC_AddToHand(int cardDrawnIndex, int playerIndex)
    {
        myDeckButton.SetActive(true);
        foreach(PhotonPlayer playerCustom in FindObjectsOfType<PhotonPlayer>())
        {
            if (PhotonNetwork.PlayerList[playerIndex] == playerCustom.GetComponent<PhotonView>().Owner)
            {
                playerCustom.myCards.Add(instance.cardsAvailable[cardDrawnIndex]);
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
        instance.cardsAvailable.Remove(instance.cardsAvailable[cardDrawnIndex]);
    }

    [PunRPC]
    void SetupDrawnCard(int playerIndex)
    {
        foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            //Cuando encontremos al owner, lo usamos para instanciar todo
            if (PhotonNetwork.PlayerList[playerIndex] == photonPlayer.GetComponent<PhotonView>().Owner && photonPlayer.GetComponent<PhotonView>().IsMine)
            {
                // Name and parenting of the card drawn. We parent it in a GO/folder to organize them easily. 
                // GameObject cardDrawn = new GameObject(myCards[drawIndex].cardNumber.ToString() + " " + myCards[drawIndex].cardSuit.ToString());
                GameObject cardDrawn = Instantiate(cardPrefab, new Vector3(0,0,0), Quaternion.identity);
                cardDrawn.transform.SetParent(myCardsFolder.transform, false);
                cardDrawn.name = photonPlayer.myCards[photonPlayer.myCards.Count - 1].cardNumber.ToString() + " " 
                                    + photonPlayer.myCards[photonPlayer.myCards.Count - 1].cardSuit.ToString();

                cardDrawn.transform.localScale = new Vector3(cardSpriteSize, cardSpriteSize, 0);
                int multiplyBy = (photonPlayer.GetNumberOfCards()-1)/maxCardsPerRow;
                cardDrawn.transform.localPosition = cardLocalPosition + 
                                    new Vector3(distanceBetweenCardsX * (photonPlayer.GetNumberOfCards() - 1 - multiplyBy * maxCardsPerRow), - distanceBetweenCardsY * multiplyBy, 0);

                // We configure it's number and suit. We'll use that info later for the game mechanics.
                cardDrawn.GetComponent<CardController>().SetCardSuit(photonPlayer.myCards[photonPlayer.myCards.Count - 1].cardSuit);
                cardDrawn.GetComponent<CardController>().SetCardNumber(photonPlayer.myCards[photonPlayer.myCards.Count - 1].cardNumber);

                // Setup the visuals
                cardDrawn.GetComponent<Image>().overrideSprite = photonPlayer.myCards[photonPlayer.myCards.Count - 1].artwork;
            }
        }
    }
}