using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenericButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    [SerializeField] AnimationClip hoverOnClip;
    [SerializeField] AnimationClip hoverOffClip;
    [SerializeField] AnimationClip selectionOnClip;
    Animator animator;
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.Play(hoverOnClip.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.Play(hoverOffClip.name);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(animator)
        {
            animator.Play(selectionOnClip.name);
        }
    }
}
