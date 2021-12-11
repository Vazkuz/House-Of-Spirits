using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] RectTransform button;
    void Start()
    {
        button.GetComponent<Animator>().Play("HoverOff_Card");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.GetComponent<Animator>().Play("HoverOn_Card");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.GetComponent<Animator>().Play("HoverOff_Card");
    }
}
