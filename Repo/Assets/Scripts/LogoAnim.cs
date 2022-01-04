using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnim : MonoBehaviour
{
    [SerializeField] MainMenuInit background;
    Animator thisAnimator;
    void Start()
    {
        thisAnimator = gameObject.GetComponent<Animator>();
    }
    public void StartSpirits()
    {
        background.StartSpiritsFadeIn();
    }

    public void FinishLogo()
    {
        thisAnimator.SetBool("FadeIn", false);
    }

}
