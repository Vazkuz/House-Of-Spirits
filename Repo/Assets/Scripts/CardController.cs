﻿using System;
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
        // thisCardButton.onClick.AddListener(TaskOnClick);
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
        // foreach(Transform child in gameController.myCards.transform)
        // {
        //     if (child.GetComponent<CardController>() != this)
        //     {
        //         child.gameObject.GetComponent<Image>().color = new Color(0,0,0);
        //     }
        // }

        // this.gameObject.GetComponent<RectTransform>().localPosition += new Vector3(0,0,0);

        if(cardNumber == 8)
        {
            GameController.gameController.card8Options.SetActive(true);
        }

        gameController.SetCardChosen(this.gameObject);
    }

    public void EnableCardHover()
    {
        hoverButton.allowHover = true;
    }

    public void DisableCardHover()
    {
        hoverButton.allowHover = false;
    }

}
