using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Deselect8Options : MonoBehaviour, ISelectHandler,IDeselectHandler
{
    [SerializeField] bool firstTime = true;
    void Start()
    {
        this.GetComponent<Button>().Select();
        firstTime = false;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        foreach(Deselect8Options deselect8Options in FindObjectsOfType<Deselect8Options>())
        {
            deselect8Options.gameObject.GetComponent<Image>().enabled = true;
        }
        PlayAnimation("SelectionOff_Image_");
        GameController.gameController.is8Selected = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(!firstTime)
        {
            PlayAnimation("StaySelected_");
            foreach(Deselect8Options deselect8Options in FindObjectsOfType<Deselect8Options>())
            {
                if(deselect8Options != this)
                {
                    deselect8Options.gameObject.GetComponent<Image>().enabled = false;
                }
            }
        }
    }    

    private void PlayAnimation(string animationPrefix)
    {
        if(GameController.gameController.cardChosen)
        {
            GameController.gameController.cardChosen.GetComponent<Animator>().Play(animationPrefix + GameController.gameController.positionInHandChosen);
        }
    }
}
