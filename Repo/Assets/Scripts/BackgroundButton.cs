using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundButton : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        if(GameController.gameController)
        {
            GameController.gameController.ClearCardChosen();
        }
    }
}
