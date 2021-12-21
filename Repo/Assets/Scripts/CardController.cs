using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{

    public int cardNumber;
    public Card.CardSuit cardSuit;
    Button thisCardButton;
    public int positionInHand;
    GameController gameController;
    HoverButton hoverButton;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        thisCardButton = GetComponent<Button>();
        hoverButton = GetComponent<HoverButton>();
    }

    public void SetCardNumber(int newCardNumber)
    {
        cardNumber = newCardNumber;
    }
    public void SetCardSuit(Card.CardSuit newCardSuit)
    {
        cardSuit = newCardSuit;
    }

    void TaskOnClick()
    {

        if(cardNumber == 8)
        {
            GameController.gameController.card8Options.SetActive(true);
        }

        gameController.SetCardChosen(this.gameObject);
    }

    public void EnableCardHover()
    {
        hoverButton.allowHover = true;
        thisCardButton.enabled = true;
    }

    public void DisableCardHover()
    {
        hoverButton.allowHover = false;
        thisCardButton.enabled = false;
    }

}
