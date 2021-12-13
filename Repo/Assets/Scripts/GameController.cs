using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gameController;
    public GameObject turnOptions;
    [SerializeField] GameObject cardOptions;
    [SerializeField] GameObject K13Options;
    public GameObject card8Options;
    public GameObject myCards;
    public GameObject cardsInGame;
    public GameObject cardChosen;
    public GameObject playButton;
    public GameObject[] suitChosenGo;
    public int SuitChosen;
    public int currentTurn = 0;
    public bool sequencePositive = true;
    public int cardChosenIndex;
    public int cardChosenIndex8;
    public bool IveDrawnACard = false;
    public bool youNeedToPlay13 = false;
    public bool ivePlayed8 = false;
    public Card.CardSuit cardSuitChosen = Card.CardSuit.NoColor;
    public int cardsToDraw = 0;
    public List<Card> cardsInGameList;
    public bool isMyTurn = false;
    [SerializeField] GameObject[] options8G0;
    public GameObject directionChanged;
    public int mySeat;
    public Image gameBackground;
    public Sprite greenBackgroud;
    public Sprite orangeBackgroud;
    public Sprite purpleBackgroud;
    public Sprite yellowBackgroud;
    public bool attemptToPlayCard = false;
    public bool card8Selected = false;
    [SerializeField] int fpsLimit = 60;

    [Header("Messages")]
    [SerializeField] GameObject alreadyDrawnCardMessage;
    [SerializeField] GameObject cantPlayCardMessage;
    [SerializeField] GameObject cantPlayCardBecause8Message;
    [SerializeField] GameObject youNeedToChooseSuitMessage;
    [SerializeField] GameObject cantPassTurnMessage;
    [SerializeField] TMP_Text kingPlayedAgainstYouMessage;

    [Header("Win screen")]
    
    public Animator winAnimator;

    public List<GameObject> allMessages;

    void Awake()
    {
        if(gameController == null)
        {
            gameController = this;
        }
        Application.targetFrameRate = fpsLimit;

    }
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        int currentScene = scene.buildIndex;
        if(currentScene == MultiplayerSettings.multiplayerSettings.gameScene)
        {
            cardOptions.SetActive(false);
            turnOptions.SetActive(false);
            K13Options.SetActive(false);
            card8Options.SetActive(false);
            youNeedToChooseSuitMessage.SetActive(false);
            directionChanged.SetActive(false);

            GameController.gameController.allMessages.Add(alreadyDrawnCardMessage);
            GameController.gameController.allMessages.Add(cantPlayCardBecause8Message);
            GameController.gameController.allMessages.Add(cantPlayCardMessage);
            GameController.gameController.allMessages.Add(youNeedToChooseSuitMessage);
            GameController.gameController.allMessages.Add(cantPassTurnMessage);
            GameController.gameController.allMessages.Add(kingPlayedAgainstYouMessage.gameObject);

            foreach(GameObject suitChosenGoElement in suitChosenGo)
            {
                suitChosenGoElement.SetActive(false);
            }

            alreadyDrawnCardMessage.SetActive(false);
            cantPlayCardMessage.SetActive(false);
            cantPlayCardBecause8Message.SetActive(false);
            cantPassTurnMessage.SetActive(false);
            kingPlayedAgainstYouMessage.gameObject.SetActive(false);
        }
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
    }

    public void CloseDeckOptions()
    {
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

        if (card8Options && !toogleValue)
        {
            card8Options.SetActive(toogleValue);
        }
        else
        {
            if(!toogleValue)
            {
                FindObjectOfType<GameController>().card8Options.SetActive(toogleValue);
            }
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

    public void AttemptToPlayCard()
    {
        attemptToPlayCard = true;
        int lastCardIndex = GameController.gameController.cardsInGameList.Count - 1;
        foreach (PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            if (photonPlayer.GetComponent<PhotonView>().IsMine
                    && photonPlayer.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[currentTurn])
            {
                if(ivePlayed8)
                {
                    if (photonPlayer.myCards[cardChosenIndex].cardSuit == cardSuitChosen
                        || photonPlayer.myCards[cardChosenIndex].cardNumber == 8)
                    {
                        if(photonPlayer.myCards[cardChosenIndex].cardNumber == 8 && SuitChosen == 0)
                        {
                            StartCoroutine(ShowInfoMessage(youNeedToChooseSuitMessage, 4f));
                        }
                        else
                        {
                            PlayCardFromHand();
                            RoomController.room.SendCardChosen8(false, 0);
                            ivePlayed8 = false;
                            foreach(GameObject suitChosenGoElement in suitChosenGo)
                            {
                                suitChosenGoElement.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        StartCoroutine(ShowInfoMessage(cantPlayCardBecause8Message, 4f));
                    }
                }
                else
                {
                    if (photonPlayer.myCards[cardChosenIndex].cardNumber == GameController.gameController.cardsInGameList[lastCardIndex].cardNumber
                        || photonPlayer.myCards[cardChosenIndex].cardSuit == GameController.gameController.cardsInGameList[lastCardIndex].cardSuit
                        || photonPlayer.myCards[cardChosenIndex].cardNumber == 8)
                    {
                        if(photonPlayer.myCards[cardChosenIndex].cardNumber == 8 && SuitChosen == 0)
                        {
                            StartCoroutine(ShowInfoMessage(youNeedToChooseSuitMessage, 4f));
                        }
                        else
                        {
                            PlayCardFromHand();
                        }
                    }
                    else
                    {
                        StartCoroutine(ShowInfoMessage(cantPlayCardMessage, 4f));
                    }
                }
            }
        }

    }

    void PlayCardFromHand()
    {
        // foreach (Transform child in myCards.transform)
        // {
        //     child.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
        // }

        foreach (PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            if (photonPlayer.GetComponent<PhotonView>().IsMine
                    && photonPlayer.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[currentTurn])
            {

                for (int lookForPlayerIndex = 0; lookForPlayerIndex < PhotonNetwork.PlayerList.Length; lookForPlayerIndex++)
                {
                    if (photonPlayer.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[lookForPlayerIndex])
                    {
                        photonPlayer.LoseCardsFromHand();
                        cardOptions.SetActive(false);
                        if(youNeedToPlay13)
                        {
                            if(photonPlayer.myCards[cardChosenIndex].cardNumber != 13)
                            {
                                InstantlyDraw13();
                            }
                            else
                            {
                                RoomController.room.PrepareSendingPlayerSequence(true, true, 3, lookForPlayerIndex);
                            }
                            youNeedToPlay13 = false;
                        }
                        if(photonPlayer.myCards[cardChosenIndex].cardNumber == 12)
                        {
                            TogglePositivenessOfSequence();
                        }
                        if(photonPlayer.myCards[cardChosenIndex].cardNumber == 8)
                        {
                            foreach(GameObject options8GOElement in GameController.gameController.options8G0)
                            {
                                options8GOElement.GetComponent<Image>().color = new Color(1,1,1);
                            }
                            GameController.gameController.card8Options.SetActive(false);
                            RoomController.room.SendCardChosen8(true, SuitChosen);
                        }
                        if(photonPlayer.myCards[cardChosenIndex].cardNumber != 2)
                        {
                            if(photonPlayer.myCards[cardChosenIndex].cardNumber == 11)
                            {
                                if(GameController.gameController.sequencePositive)
                                {
                                    GameController.gameController.currentTurn++;
                                }
                                else
                                {
                                    GameController.gameController.currentTurn--;
                                }
                                if (GameController.gameController.currentTurn >= PhotonNetwork.PlayerList.Length)
                                {
                                    GameController.gameController.currentTurn = 0;
                                }
                                else if (GameController.gameController.currentTurn < 0)
                                {
                                    GameController.gameController.currentTurn = PhotonNetwork.PlayerList.Length - 1;
                                }
                            }
                            GoToNextPlayerTurn(photonPlayer, lookForPlayerIndex);
                        }
                        GameController.gameController.IveDrawnACard = false;
                        GameController.gameController.playButton.SetActive(true);
                        photonPlayer.SendCardFromHandToTable(cardChosenIndex, lookForPlayerIndex);
                    }
                }
            }
        }
        // for(int childIndex = 0; childIndex < CardDisplay.cardDisplayInstance.myCardsFolder.transform.childCount; childIndex++)
        // {
        //     if(CardDisplay.cardDisplayInstance.myCardsFolder.transform.GetChild(childIndex).gameObject.activeInHierarchy)
        //     {
        //         CardDisplay.cardDisplayInstance.indexCardOnLeft = childIndex;
        //         break;
        //     }
        // }
        Debug.Log("La carta de la izquierda es... " + CardDisplay.cardDisplayInstance.indexCardOnLeft);
    }

    void GoToNextPlayerTurn(PhotonPlayer photonPlayer, int playerIndex)
    {
        if(GameController.gameController.sequencePositive)
        {
            GameController.gameController.currentTurn++;
        }
        else
        {
            GameController.gameController.currentTurn--;
        }
        if (GameController.gameController.currentTurn >= PhotonNetwork.PlayerList.Length)
        {
            GameController.gameController.currentTurn = 0;
        }
        else if (GameController.gameController.currentTurn < 0)
        {
            GameController.gameController.currentTurn = PhotonNetwork.PlayerList.Length - 1;
        }
        CloseDeckOptions();
        if(photonPlayer.myCards[cardChosenIndex].cardNumber != 13)
        {
            RoomController.room.PrepareSendingPlayerSequence(true, false, 0, playerIndex);
        }
        else
        {
            RoomController.room.PrepareSendingPlayerSequence(true, true, GameController.gameController.cardsToDraw + 3, playerIndex);
        }
        youNeedToPlay13 = false;
    }

    public void PassTurn()
    {
        foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            if (PhotonNetwork.PlayerList[GameController.gameController.currentTurn] == photonPlayer.GetComponent<PhotonView>().Owner &&
                    photonPlayer.GetComponent<PhotonView>().IsMine)
            {
                if(!GameController.gameController.IveDrawnACard)
                {
                    StartCoroutine(ShowInfoMessage(cantPassTurnMessage, 3.5f));
                }
                else
                {   
                    if(GameController.gameController.sequencePositive)
                    {
                        GameController.gameController.currentTurn++;
                    }
                    else
                    {
                        GameController.gameController.currentTurn--;
                    }
                    if (GameController.gameController.currentTurn >= PhotonNetwork.PlayerList.Length)
                    {
                        GameController.gameController.currentTurn = 0;
                    }
                    else if (GameController.gameController.currentTurn < 0)
                    {
                        GameController.gameController.currentTurn = PhotonNetwork.PlayerList.Length - 1;
                    }
                    RoomController.room.PrepareSendingPlayerSequence(true, false, 0, GameController.gameController.currentTurn);
                    
                    GameController.gameController.IveDrawnACard = false;
                }
            }
        }
    }

    public void DrawSingleCard()
    {
        if (!GameController.gameController.IveDrawnACard)
        {
            if(!CardDisplay.cardDisplayInstance.rightButton.activeInHierarchy && CardDisplay.cardDisplayInstance.myCardsFolder.transform.childCount > CardDisplay.cardDisplayInstance.maxCardsPerRow)
            {
                CardDisplay.cardDisplayInstance.rightButton.SetActive(true);
            }
            foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
            {
                if (PhotonNetwork.PlayerList[GameController.gameController.currentTurn] == photonPlayer.GetComponent<PhotonView>().Owner &&
                        photonPlayer.GetComponent<PhotonView>().IsMine)
                {
                    if(youNeedToPlay13)
                    {
                        CardDisplay.cardDisplayInstance.DrawCards(GameController.gameController.cardsToDraw, photonPlayer, GameController.gameController.currentTurn, false);
                        GameController.gameController.cardsToDraw = 0;
                        GameController.gameController.IveDrawnACard = false;
                        youNeedToPlay13 = false;
                    }
                    else
                    {
                        CardDisplay.cardDisplayInstance.DrawCards(1, photonPlayer, GameController.gameController.currentTurn, false);
                    }
                }
            }
            GameController.gameController.IveDrawnACard = true;
            CardDisplay cardDisplay = CardDisplay.cardDisplayInstance;
            if(!cardDisplay.myCardsFolder.transform.GetChild(cardDisplay.myCardsFolder.transform.childCount-1).gameObject.activeInHierarchy)
            {
                cardDisplay.rightButton.gameObject.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(ShowInfoMessage(alreadyDrawnCardMessage, 3f));
        }
    }

    IEnumerator ShowInfoMessage(GameObject message, float time)
    {
        foreach(GameObject messageGO in GameController.gameController.allMessages)
        {
            messageGO.SetActive(false);
        }
        message.SetActive(true);
        yield return new WaitForSeconds(time);
        message.SetActive(false);
    }

    public void SomeoneWantsMeToDraw(bool makeNextPlayerDraw, int cardsToDraw, PhotonPlayer playerCustom, int playerIndex)
    {
        if(!youNeedToPlay13)
        {
            kingPlayedAgainstYouMessage.text = "Someone played King against you. You have to either play another King or draw " 
                                                + GameController.gameController.cardsToDraw + " cards.";
            K13Options.SetActive(true);
            StartCoroutine(ShowInfoMessage(kingPlayedAgainstYouMessage.gameObject, 5f));
        }
    }

    public void PlayAfter13()
    {
        K13Options.SetActive(false);
        youNeedToPlay13 = true;
    }

    public void InstantlyDraw13()
    {
        K13Options.SetActive(false);
        foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            if (PhotonNetwork.PlayerList[GameController.gameController.currentTurn] == photonPlayer.GetComponent<PhotonView>().Owner &&
                    photonPlayer.GetComponent<PhotonView>().IsMine)
            {
                CardDisplay.cardDisplayInstance.DrawCards(GameController.gameController.cardsToDraw, photonPlayer, GameController.gameController.currentTurn, false); 
            }
        }
        GameController.gameController.cardsToDraw = 0;
        GameController.gameController.IveDrawnACard = false;
        youNeedToPlay13 = false;
    }

    void TogglePositivenessOfSequence()
    {
        GameController.gameController.sequencePositive = !GameController.gameController.sequencePositive;
        RoomController.room.SendSequenceOrder(GameController.gameController.sequencePositive);
    }

    public void IvePlayed8(int suitChosen)
    {
        // foreach(GameObject options8GOElement in GameController.gameController.options8G0)
        // {
        //     options8GOElement.GetComponent<Image>().color = new Color(0,0,0);
        // }
        // options8G0[suitChosen-1].GetComponent<Image>().color = new Color(1,1,1);

        if (suitChosen == 1)
        {
            cardSuitChosen = Card.CardSuit.Orange;
        }
        else if (suitChosen == 2)
        {
            cardSuitChosen = Card.CardSuit.Yellow;
        }
        else if (suitChosen == 3)
        {
            cardSuitChosen = Card.CardSuit.Green;
        }
        else if (suitChosen == 4)
        {
            cardSuitChosen = Card.CardSuit.Purple;
        }

        SuitChosen = suitChosen;
    }

}
