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
        PlayAnimation("SelectionOff_Image_");
        GameController.gameController.is8Selected = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(!firstTime)
        {
            PlayAnimation("StaySelected_");
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
