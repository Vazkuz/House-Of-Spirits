using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritsFade : MonoBehaviour
{
    [SerializeField] MainMenuInit background;
    Animator thisAnimator;
    void Start()
    {
        thisAnimator = gameObject.GetComponent<Animator>();
    }

    public void FinishFade()
    {
        thisAnimator.SetBool("FadeIn", false);
    }

    public void ShowEverything()
    {
        background.ShowEverything();
    }

}
