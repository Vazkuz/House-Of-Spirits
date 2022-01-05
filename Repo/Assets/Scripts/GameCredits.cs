using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCredits : MonoBehaviour
{
    [SerializeField] MainMenuInit background;
    Animator thisAnimator;
    void Start()
    {
        thisAnimator = gameObject.GetComponent<Animator>();
    }

    public void ShowEverything()
    {
        background.ShowEverything();
    }    

    public void FinishGameCredits()
    {
        thisAnimator.SetBool("FadeIn", false);
    }

}
