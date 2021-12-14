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
    public int is8Selected = 0;
    void Start()
    {
        Debug.Log("Start");
        button.GetComponent<Animator>().Play("HoverOff_Card");
        button.transform.GetChild(0).transform.GetComponent<Image>().sprite = button.GetComponent<Image>().sprite;
        foreach(GameObject option8 in GameObject.FindGameObjectsWithTag("8option"))
        {
            option8.GetComponent<Image>().enabled = false;
            option8.GetComponent<Button>().enabled = false;
            option8.GetComponent<Button>().Select();
        }
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

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("OnSelect");
        cardSelected = true;
        GameController.gameController.SetCardChosen(eventData.selectedObject);
        if(eventData.selectedObject.GetComponent<CardController>())
        {
            PlayAnimation(eventData, "SelectionOn_Image_");
        }

        if (eventData.selectedObject.GetComponent<CardController>().cardNumber == 8)
        {
            foreach(GameObject option8 in GameObject.FindGameObjectsWithTag("8option"))
            {
                option8.GetComponent<Image>().enabled = true;
                option8.GetComponent<Button>().enabled = true;
                option8.GetComponent<Button>().Select();
            }
            is8Selected = 4;
        }

    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("OnDeselect");
        cardSelected = false;
        if(eventData.selectedObject.GetComponent<CardController>())
        {
            PlayAnimation(eventData, "SelectionOff_Image_");
        }
        // if(eventData.selectedObject.GetComponent<CardController>().cardNumber != 8)
        // {
        //     GameController.gameController.ClearCardChosen();
        // }
    }

    private void PlayAnimation(BaseEventData eventData, string animationPrefix)
    {
        button.GetComponent<Animator>().Play(animationPrefix + eventData.selectedObject.GetComponent<CardController>().positionInHand);

    }
}














    
        //Select
        // if (eventData.selectedObject.GetComponent<CardController>().positionInHand == 0)
        // {
        //     button.GetComponent<Animator>().Play("SelectionOn_Image_0");
        // }
        // else if (eventData.selectedObject.GetComponent<CardController>().positionInHand == 1)
        // {
        //     button.GetComponent<Animator>().Play("SelectionOn_Image_1");
        // }
        // else if (eventData.selectedObject.GetComponent<CardController>().positionInHand == 2)
        // {
        //     button.GetComponent<Animator>().Play("SelectionOn_Image_2");
        // }
        // else if (eventData.selectedObject.GetComponent<CardController>().positionInHand == 3)
        // {
        //     button.GetComponent<Animator>().Play("SelectionOn_Image_3");
        // }
        // else if (eventData.selectedObject.GetComponent<CardController>().positionInHand == 4)
        // {
        //     button.GetComponent<Animator>().Play("SelectionOn_Image_4");
        // }

        //Deselect
        // if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 0)
        // {
        //     button.GetComponent<Animator>().Play("SelectionOff_Image_0");
        // }
        // else if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 1)
        // {
        //     button.GetComponent<Animator>().Play("SelectionOff_Image_1");
        // }
        // else if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 2)
        // {
        //     button.GetComponent<Animator>().Play("SelectionOff_Image_2");
        // }
        // else if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 3)
        // {
        //     button.GetComponent<Animator>().Play("SelectionOff_Image_3");
        // }
        // else if(eventData.selectedObject.GetComponent<CardController>().positionInHand == 4)
        // {
        //     button.GetComponent<Animator>().Play("SelectionOff_Image_4");
        // }
