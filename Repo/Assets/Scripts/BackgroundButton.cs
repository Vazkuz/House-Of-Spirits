using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackgroundButton : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        if(GameController.gameController)
        {
            GameController.gameController.ClearCardChosen();
        }

        foreach(GameObject option8 in GameObject.FindGameObjectsWithTag("8option"))
        {
            option8.GetComponent<Image>().enabled = false;
            option8.GetComponent<Button>().enabled = false;
        }
    }
}
