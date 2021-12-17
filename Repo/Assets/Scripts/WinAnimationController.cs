using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinAnimationController : MonoBehaviour
{
    public static WinAnimationController winAnimationController;

    void Awake()
    {
        if (WinAnimationController.winAnimationController == null)
        {
            WinAnimationController.winAnimationController = this;
        }
        else
        {
            if (WinAnimationController.winAnimationController != this)
            {
                Destroy(WinAnimationController.winAnimationController.gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
        this.GetComponent<Canvas>().enabled = false;
    }
}
