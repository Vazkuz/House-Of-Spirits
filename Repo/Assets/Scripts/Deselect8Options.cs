using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deselect8Options : MonoBehaviour, IDeselectHandler
{
    public void OnDeselect(BaseEventData eventData)
    {
        GameController.gameController.card8Options.SetActive(false);
        GameController.gameController.cardSuitChosen = Card.CardSuit.NoColor;
    }
}
