using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] bool firstTime = true;
    Deselect8Options[] options8;
    void Start()
    {
        options8 = FindObjectsOfType<Deselect8Options>();
        this.GetComponent<Button>().Select();
        firstTime = false;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        GameController.gameController.ClearCardChosen();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(!firstTime)
        {
            Debug.Log("Starting play");
            // int numberOfOptionsSelected = 0;
            // if(GameController.gameController.cardChosen.GetComponent<Card>().cardNumber == 8)
            // {
            //     foreach(Deselect8Options option8 in options8)
            //     {
            //         if(EventSystem.current.currentSelectedGameObject == option8.GetComponent<Button>()) numberOfOptionsSelected++;
            //     }
            //     if(numberOfOptionsSelected <= 0)
            //     {
            //         print("No has seleccionado nada");
            //     }
            //     else
            //     {
            //         print("sí seleccionaste");
            //     }
            // }
            //Play card
            GameController.gameController.AttemptToPlayCard();
            //delete cardChosen
            GameController.gameController.ClearCardChosen();
            foreach(GameObject option8 in GameObject.FindGameObjectsWithTag("8option"))
            {
                option8.GetComponent<Image>().enabled = false;
                option8.GetComponent<Button>().enabled = false;
                option8.GetComponent<Button>().Select();
            }
        }
    }
}
