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
        Debug.Log("Start");
        button.GetComponent<Animator>().Play("HoverOff_Card");
        button.transform.GetChild(0).transform.GetComponent<Image>().sprite = button.GetComponent<Image>().sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        if(!cardSelected)
        {
            button.GetComponent<Animator>().Play("HoverOn_Card");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
        if(!cardSelected)
        {
            button.GetComponent<Animator>().Play("HoverOff_Card");
        }
    }   

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("OnDeselect");
        cardSelected = false;
        if(eventData.selectedObject.GetComponent<CardController>())
        {
            if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 0)
            {
                button.GetComponent<Animator>().Play("SelectionOff_Image_0");
            }
            else if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 1)
            {
                button.GetComponent<Animator>().Play("SelectionOff_Image_1");
            }
            else if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 2)
            {
                button.GetComponent<Animator>().Play("SelectionOff_Image_2");
            }
            else if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 3)
            {
                button.GetComponent<Animator>().Play("SelectionOff_Image_3");
            }
            else if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 4)
            {
                button.GetComponent<Animator>().Play("SelectionOff_Image_4");
            }
        }
    }


    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("OnSelect");
        cardSelected = true;
        GameController.gameController.SetCardChosen(eventData.selectedObject);
        if(eventData.selectedObject.GetComponent<CardController>())
        {
            if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 0)
            {
                button.GetComponent<Animator>().Play("SelectionOn_Image_0");
            }
            else if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 1)
            {
                button.GetComponent<Animator>().Play("SelectionOn_Image_1");
            }
            else if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 2)
            {
                button.GetComponent<Animator>().Play("SelectionOn_Image_2");
            }
            else if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 3)
            {
                button.GetComponent<Animator>().Play("SelectionOn_Image_3");
            }
            else if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 4)
            {
                button.GetComponent<Animator>().Play("SelectionOn_Image_4");
            }
        }
    }
}
