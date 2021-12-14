using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deselect8Options : MonoBehaviour, ISelectHandler,IDeselectHandler
{
    public void OnDeselect(BaseEventData eventData)
    {
        if(GameController.gameController.attemptToPlayCard)
        {
            StartCoroutine(WaitAndToggle());
        }
        else
        {
            WaitAndToggle();
        }
    }

    IEnumerator WaitAndToggle()
    {
        for(int i=0;i<8;i++)
        {
            yield return null;
        }
        GameController.gameController.cardSuitChosen = Card.CardSuit.NoColor;
        GameController.gameController.ivePlayed8 = false;
        GameController.gameController.card8Options.SetActive(false);
        GameController.gameController.ToggleCardOptions(false); //no lo necesito
        GameController.gameController.playButton.SetActive(false); //no lo necesito
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(YaNoQuieroHacerEsto());
    }

    IEnumerator YaNoQuieroHacerEsto()
    {
        for(int i_frames = 0; i_frames < 10; i_frames++)
        {
            yield return null;
        }
        GameController.gameController.playButton.SetActive(true);
    }
}
