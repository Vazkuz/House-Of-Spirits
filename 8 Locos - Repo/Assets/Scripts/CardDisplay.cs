using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    //public Card card;
    List<Card> cardsAvailable;
    List<Card> myCards = new List<Card>();
    // Start is called before the first frame update
    void Start()
    {
        cardsAvailable = new List<Card>(Resources.LoadAll<Card>("Cards")); //"Cards" es el path OJO. (Dentro de Resources)
        DrawCards(8);
    }

    private void DrawCards(int cardsToDrawn)
    {
        for (int drawIndex = 1; drawIndex <= cardsToDrawn; drawIndex++)
        {
            int cardDrawn = Random.Range(1, cardsAvailable.Count + 1);
            Debug.Log(cardsAvailable[cardDrawn]);
            Debug.Log(cardsAvailable[cardDrawn].cardNumber + " " + cardsAvailable[cardDrawn].cardSuit.ToString());
            myCards.Add(cardsAvailable[cardDrawn]);
            cardsAvailable.Remove(cardsAvailable[cardDrawn]);
        }
    }
}
