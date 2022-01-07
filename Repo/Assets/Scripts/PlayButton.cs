using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] bool firstTime = true;
    Deselect8Options[] options8;
    Image thisImage;
    void Awake()
    {
        thisImage = gameObject.GetComponent<Image>();
    }
    void Start()
    {
        options8 = FindObjectsOfType<Deselect8Options>();
        this.GetComponent<Button>().Select();
        firstTime = false;
    }

    public void EnableImage()
    {
        thisImage.enabled = true;
    }

    public void DisableImage()
    {
        thisImage.enabled = false;
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
