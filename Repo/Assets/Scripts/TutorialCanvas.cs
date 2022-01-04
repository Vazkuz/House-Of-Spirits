using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    Animator canvasAnimator;
    void Start()
    {
        canvasAnimator = this.gameObject.GetComponent<Animator>();
    }
    public void BackToNoAnimation()
    {
        // canvasAnimator.SetInteger("Fade", 0);
    }
}
