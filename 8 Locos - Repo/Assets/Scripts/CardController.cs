using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{

    [SerializeField] int cardNumber;
    [SerializeField] Card.CardSuit cardSuit;

    public void SetCardNumber(int newCardNumber)
    {
        cardNumber = newCardNumber;
    }
    public void SetCardSuit(Card.CardSuit newCardSuit)
    {
        cardSuit = newCardSuit;
    }

}
