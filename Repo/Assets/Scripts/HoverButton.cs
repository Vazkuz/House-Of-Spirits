using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] RectTransform button;
    bool cardSelected = false;
    void Start()
    {
        button.GetComponent<Animator>().Play("HoverOff_Card");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!cardSelected)
        {
            button.GetComponent<Animator>().Play("HoverOn_Card");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!cardSelected)
        {
            button.GetComponent<Animator>().Play("HoverOff_Card");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Carta seleccionada");
        cardSelected = true;
        button.GetComponent<Animator>().Play("SelectionOn_Card");
        if (eventData.selectedObject.GetComponent<CardController>().cardNumber == 8)
        {
            GameController.gameController.card8Options.SetActive(true);
            foreach (PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
            {
                if (photonPlayer.GetComponent<PhotonView>().IsMine
                        && photonPlayer.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[GameController.gameController.currentTurn])
                {
                    GameController.gameController.cardChosenIndex8 = GameController.gameController.cardChosenIndex;
                }
            }
        }
        StartCoroutine(YaNoQuieroHacerEsto2());
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("Carta deseleccionada");
        cardSelected = false;
        button.GetComponent<Animator>().Play("HoverOff_Card");
        if(eventData.selectedObject.GetComponent<CardController>().cardNumber == 8)
        {
            StartCoroutine(YaNoQuieroHacerEsto());
        }
        StartCoroutine(YaNoQuieroHacerEsto3());
    }

    IEnumerator YaNoQuieroHacerEsto2()
    {
        for(int i_frames = 0; i_frames < 10; i_frames++)
        {
            yield return null;
        }
        GameController.gameController.playButton.SetActive(true);
    }

    IEnumerator YaNoQuieroHacerEsto3()
    {
        for(int i_frames = 0; i_frames < 8; i_frames++)
        {
            yield return null;
        }
        GameController.gameController.playButton.SetActive(false);
    }

    IEnumerator YaNoQuieroHacerEsto()
    {
        for(int i_frames = 0; i_frames < 8; i_frames++)
        {
            yield return null;
        }
        if (GameController.gameController.cardSuitChosen == Card.CardSuit.NoColor)
        {
            GameController.gameController.card8Options.SetActive(false);
        }
    }
}