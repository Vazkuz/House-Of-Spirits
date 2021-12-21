using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    public GameObject myCardsFolder;
    [SerializeField] float cardSpriteSize = 1f;
    [SerializeField] int maxPlayersJustOneDeck;
    public GameObject rightButton;
    public GameObject leftButton;
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
        leftButton.SetActive(false);
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
                    DrawCards(cardsDrawnInitially, playerCustom, playerIndex, true);
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

    public void DrawCards(int cardsToDrawn, PhotonPlayer photonPlayer, int playerIndex, bool firstDraw)
    {
        if(cardDisplayInstance.cardsAvailable.Count == 0)
        {
            PV.RPC("MixCardsInGame",RpcTarget.All);
        }

        for (int drawIndex = 0; drawIndex < cardsToDrawn; drawIndex++)
        {
            int cardDrawnIndex = Random.Range(0, cardDisplayInstance.cardsAvailable.Count);
            PV.RPC("RPC_AddToHand",RpcTarget.All,cardDrawnIndex, playerIndex);
            if(firstDraw)
            {
                PV.RPC("SetupDrawnCard", RpcTarget.All,playerIndex, drawIndex);
            }
            else
            {
                int cardsShown = 0;
                for(int childIndex = 0; childIndex < myCardsFolder.transform.childCount; childIndex++)
                {
                    if(myCardsFolder.transform.GetChild(childIndex).gameObject.activeInHierarchy)
                    {
                        cardsShown++;
                    }
                }
                PV.RPC("SetupDrawnCard", RpcTarget.All,playerIndex, cardsShown);//photonPlayer.cardsIHave-1);
            }
            PV.RPC("RPC_RemoveFromDeck",RpcTarget.All,cardDrawnIndex);
        }

        // int cardsShown2 = 0;
        // for(int childIndex = 0; childIndex < CardDisplay.cardDisplayInstance.myCardsFolder.transform.childCount; childIndex++)
        // {
        //     if(CardDisplay.cardDisplayInstance.myCardsFolder.transform.GetChild(childIndex).gameObject.activeInHierarchy)
        //     {
        //         cardsShown2++;
        //     }
        // }
        // if(rightButton.activeInHierarchy || cardsShown2 >= CardDisplay.cardDisplayInstance.maxCardsPerRow)
        // {
        //     CardDisplay.cardDisplayInstance.rightButton.SetActive(true);
        // }
        // else
        // {
        //     CardDisplay.cardDisplayInstance.rightButton.SetActive(false);
        // }
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

    private void ArrangeCards(int drawIndex, GameObject cardDrawn)
    {
        for(int playerIndex = 0; playerIndex < PhotonNetwork.PlayerList.Length; playerIndex++)
        {
            foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
            {
                //Cuando encontremos al owner, lo usamos para instanciar todo
                if (PhotonNetwork.PlayerList[playerIndex] == photonPlayer.GetComponent<PhotonView>().Owner && photonPlayer.GetComponent<PhotonView>().IsMine)
                {
                    if (drawIndex < maxCardsPerRow)
                    {
                        cardDrawn.gameObject.SetActive(true);
                        cardDrawn.transform.localPosition = new Vector3(-((maxCardsPerRow / 2 - drawIndex) * distanceBetweenCardsX), 0, 0);
                        cardDrawn.GetComponent<CardController>().positionInHand = drawIndex;
                    }
                    else
                    {
                        //cardDrawn.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void MoveToRightCards()
    {
        indexCardOnLeft++;
        for(int indexCard = 0; indexCard < indexCardOnLeft; indexCard++)
        {
            myCardsFolder.transform.GetChild(indexCard).gameObject.SetActive(false);
        }

        for(int indexCard = indexCardOnLeft; indexCard < myCardsFolder.transform.childCount; indexCard++)
        {
            Transform child = child = myCardsFolder.transform.GetChild(indexCard);
            if (indexCard < CardDisplay.cardDisplayInstance.maxCardsPerRow + indexCardOnLeft)
            {
                child.gameObject.SetActive(true);
                child.localPosition = new Vector3(-((CardDisplay.cardDisplayInstance.maxCardsPerRow / 2 - (indexCard-indexCardOnLeft)) * CardDisplay.cardDisplayInstance.distanceBetweenCardsX), 0, 0);
                child.gameObject.GetComponent<CardController>().positionInHand = indexCard-indexCardOnLeft;
                // child.GetComponent<Animator>().Play("HoverOff_Card");
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }

        if(indexCardOnLeft+maxCardsPerRow >= myCardsFolder.transform.childCount)
        {
            rightButton.gameObject.SetActive(false);
        }

        if(indexCardOnLeft > 0)
        {
            leftButton.gameObject.SetActive(true);
        }

    }

    public void MoveToLefttCards()
    {
        indexCardOnLeft--;
        for(int indexCard = 0; indexCard < indexCardOnLeft; indexCard++)
        {
            myCardsFolder.transform.GetChild(indexCard).gameObject.SetActive(false);
        }

        for(int indexCard = indexCardOnLeft; indexCard < myCardsFolder.transform.childCount; indexCard++)
        {
            Transform child = child = myCardsFolder.transform.GetChild(indexCard);
            if (indexCard < CardDisplay.cardDisplayInstance.maxCardsPerRow + indexCardOnLeft)
            {
                child.gameObject.SetActive(true);
                child.localPosition = new Vector3(-((CardDisplay.cardDisplayInstance.maxCardsPerRow / 2 - (indexCard-indexCardOnLeft)) * CardDisplay.cardDisplayInstance.distanceBetweenCardsX), 0, 0);
                child.gameObject.GetComponent<CardController>().positionInHand = indexCard-indexCardOnLeft;
                // child.GetComponent<Animator>().Play("HoverOff_Card");
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }

        if(indexCardOnLeft <= 0)
        {
            leftButton.gameObject.SetActive(false);
        }       

        if(indexCardOnLeft+maxCardsPerRow < myCardsFolder.transform.childCount)
        {
            rightButton.gameObject.SetActive(true);
        }

    }

}
