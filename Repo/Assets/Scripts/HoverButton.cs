using System.Collections;
using System.Collections.Generic;
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
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("Carta deseleccionada");
        cardSelected = false;
        button.GetComponent<Animator>().Play("HoverOff_Card");
    }
}
