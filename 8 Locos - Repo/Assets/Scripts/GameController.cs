using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController gameController;
    public GameObject deckCanvas;
    [SerializeField] GameObject openDeckButton;
    [SerializeField] GameObject cardOptions;
    public GameObject myCards;
    public GameObject cardsInGame;
    public GameObject cardChosen;

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
    }

    public void SetCardChosen(GameObject cardClicked)
    {
        cardChosen = cardClicked;
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
        }
    }

    public void PlayCardFromHand()
    {
        gameController.cardChosen.transform.SetParent(cardsInGame.transform);
        cardChosen.transform.localPosition = new Vector3(0f,0f,0f);
        cardChosen.GetComponent<Button>().enabled = false;
        foreach(Transform child in myCards.transform)
        {
            child.gameObject.GetComponent<Image>().color = new Color(1,1,1);
        }
        deckCanvas.SetActive(false);
        cardOptions.SetActive(false);

        foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            if(photonPlayer.GetComponent<PhotonView>().IsMine)
            {
                photonPlayer.LoseCardsFromHand();
            }
        }

    }

}
