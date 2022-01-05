using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterType : MonoBehaviour, ISelectHandler
{
    public int characterIndex;
    public string characterName;
    public Sprite spriteInRoom;
    public Sprite spriteInGame;
    public Sprite spriteDisconnected;
    public Sprite spriteTurn;
    public Sprite notSelected;
    public Sprite selectedByOther;
    public Sprite selectedByMe;
    public int cardsIHave;

    void Start()
    {
    }

    public void ChangeImageInButtonAvatar(Sprite whichTypeOfSelection)
    {
        this.GetComponent<Image>().sprite = whichTypeOfSelection;
    }

    void ShowNumberOfCards()
    {
        if(RoomController.room.currentScene == MultiplayerSettings.multiplayerSettings.gameScene)
        {
            string textForPlayer = "That player has " + cardsIHave + " cards";
            GameController.gameController.numberOfCardsOfPlayer.GetComponent<TMP_Text>().text = textForPlayer;
            GameController.gameController.numberOfCardsOfPlayerShadow.GetComponent<TMP_Text>().text = textForPlayer;
            GameController.gameController.ShowNumberOfCardsOfPlayer();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowNumberOfCards();
    }
}
