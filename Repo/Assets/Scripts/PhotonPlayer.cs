using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhotonPlayer : MonoBehaviour
{
    [SerializeField] float avatarOffsetX;
    [SerializeField] float avatarOffsetY;
    [SerializeField] float nicknameOffsetX;
    [SerializeField] float nicknameOffsetY;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] float avatarScale = 1.7f;
    public int cardsIHave;
    public GameObject[] allCharacters;
    // public string[] avatarNames;
    public string mySelectedName;
    public PhotonView PV;
    public GameObject myAvatar;
    public int mySelectedCharacter;
    public int myPositionInGrid;
    public bool IWon = false;
    public List<Card> myCards = new List<Card>();
    TMP_Text numberOfCardsText;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        numberOfCardsText = GetComponent<TMP_Text>();
        if(PV.IsMine)
        {
            //Check if player has a nickname. If not, default nickname will be "Player"
            if (!PlayerPrefs.HasKey("MY_NICKNAME"))
            {
                PlayerPrefs.SetString("MY_NICKNAME", "Player");
            }
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.IsLocal)
                {
                    player.NickName = PlayerPrefs.GetString("MY_NICKNAME");
                }
            }


            // //Check if player has selected a color before. If not, default color will be white (identified with number 0)
            // if (PlayerPrefs.HasKey("MY_CHARACTER"))
            // {
            //     GetComponent<PhotonPlayer>().mySelectedCharacter = PlayerPrefs.GetInt("MY_CHARACTER");
            // }
            // else
            // {
            StartCoroutine(SetInitialAvatarLag());
        }
    }

    IEnumerator SetInitialAvatarLag()
    {
        yield return new WaitForSeconds(1f);
        for (int indexAvatarsTaken = 0; indexAvatarsTaken < PhotonNetwork.CurrentRoom.PlayerCount; indexAvatarsTaken++)
        {
            if (!RoomController.room.avatarsTaken.Contains(indexAvatarsTaken))
            {
                PV.RPC("RPC_AddAvatarTaken", RpcTarget.AllBuffered, indexAvatarsTaken, ((int)PhotonNetwork.CurrentRoom.PlayerCount) - 1);
                break;
            }
        }

        PV.RPC("RPC_InstantiateAvatar", RpcTarget.AllBuffered, PlayerInfo.PI.mySpaceInGrid, GetComponent<PhotonPlayer>().mySelectedCharacter);

        Debug.Log("Starting disabling of buttons");
        RoomController.room.DisableAvatarsTaken();
    }

    [PunRPC]
    void RPC_AddAvatarTaken(int indexAvatar, int indexPlayer)
    {
        RoomController.room.avatarsTaken.Add(indexAvatar);
        foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            if(PhotonNetwork.PlayerList[indexPlayer] == photonPlayer.GetComponent<PhotonView>().Owner)
            {
                photonPlayer.mySelectedCharacter = indexAvatar;
                photonPlayer.mySelectedName = AvatarPreviewController.APC.avatarNames[indexAvatar];
            }
        }
    }

    [PunRPC]
    void RPC_RemoveAvatarTaken(int indexAvatar)
    {
        Debug.Log("The avatar "+ indexAvatar + " will change");
        RoomController.room.avatarsTaken.Remove(indexAvatar);
    }

    [PunRPC]
    void RPC_InstantiateAvatar(int positionOfAvatar, int mySelectedCharacter)
    {
        StartCoroutine(InstantiateWithLag(positionOfAvatar, mySelectedCharacter));
    }

    IEnumerator InstantiateWithLag(int positionOfAvatar, int mySelectedCharacter)
    {
        yield return new WaitForSeconds(0.5f);
        GameObject myAvatar = Instantiate(allCharacters[mySelectedCharacter], transform.position, Quaternion.identity) as GameObject;
        myAvatar.transform.localScale = new Vector3(avatarScale, avatarScale, 1f);
        myAvatar.transform.SetParent(transform, false);
        transform.SetParent(PlayerInfo.PI.allSpacesInGrid[positionOfAvatar].transform, false);
        myPositionInGrid = positionOfAvatar;
        myAvatar.GetComponent<RectTransform>().localPosition = new Vector3(avatarOffsetX, avatarOffsetY, 0);
        PlayerInfo.PI.nicknameFrames[positionOfAvatar].SetActive(true);
        PlayerInfo.PI.godnameFrames[positionOfAvatar].SetActive(true);
    }

    public void ChangePlayerAvatar(int whichCharacter)
    {        
        if (PlayerInfo.PI != null)
        {
            Photon.Realtime.Player[] playersInRoom = PhotonNetwork.PlayerList;
            PhotonPlayer[] playersInRoomCustom = FindObjectsOfType<PhotonPlayer>();
            Debug.Log("mySelectedCharacter = " + GetComponent<PhotonPlayer>().mySelectedCharacter);
            foreach (PhotonPlayer playerInRoomCustom in playersInRoomCustom)
            {
                if(playerInRoomCustom.PV.IsMine)
                {
                    playerInRoomCustom.PV.RPC("RPC_RemoveAvatarTaken", RpcTarget.AllBuffered, playerInRoomCustom.mySelectedCharacter);
                    playerInRoomCustom.mySelectedCharacter = whichCharacter;
                    playerInRoomCustom.PV.RPC("RPC_AddAvatarTaken", RpcTarget.AllBuffered, playerInRoomCustom.mySelectedCharacter, playerInRoomCustom.myPositionInGrid);
                    RoomController.room.DisableAvatarsTaken();
                }
            }
            //PlayerPrefs.SetInt("MY_CHARACTER", whichCharacter);
            // PhotonPlayer playerCustom;
            // foreach(PhotonPlayer player in playersInRoom)

            //Primero buscamos entre todos los PV cuál es el "mío".
            foreach (PhotonPlayer playerInRoomCustom in playersInRoomCustom)
            {
                //Cuando encontramos el "mío". Buscaremos su índice en la Lista de Jugadores de Photon.
                if(playerInRoomCustom.PV.IsMine)
                {
                    //Empezamos a buscar entre los jugadores de la lista de photon cuál es el índex "nuestro".
                    for(int playerIndex = 0; playerIndex < playersInRoom.Length; playerIndex++)
                    {
                        if (playerInRoomCustom.PV.Owner == playersInRoom[playerIndex]) //Este es el índex! Con él podemos identificar unívocamente al jugador sin confusiones
                        {
                            playerInRoomCustom.mySelectedCharacter = whichCharacter;
                            playerInRoomCustom.PV.RPC("SendNewColorToAllPlayers",RpcTarget.AllBuffered,playerIndex, playerInRoomCustom.mySelectedCharacter);
                        }                        
                    }
                    // playerInRoomCustom.PV.RPC("SendNewColorToAllPlayers",RpcTarget.AllBuffered,playerIndex, mySelectedCharacter);
                }
            }

        }
    }

    [PunRPC]
    private void SendNewColorToAllPlayers(int playerIndex, int mySelectedCharacter)
    {
        Debug.Log("Changing avatar color");
        //Aquí tendremos que volver a buscar el PhotonNetwork Player del jugador D:
        foreach(PhotonPlayer playerCustom in FindObjectsOfType<PhotonPlayer>())
        {            
            if(playerCustom.PV != null)
            {
                //Cuando encontremos al owner, lo usamos para instanciar todo
                if (PhotonNetwork.PlayerList[playerIndex] == playerCustom.PV.Owner)
                {
                    GameObject myAvatar = Instantiate(allCharacters[mySelectedCharacter], transform.position, Quaternion.identity);
                    myAvatar.transform.localScale = new Vector3(avatarScale, avatarScale, 1f);
                    if(playerCustom.transform.childCount > 2) Destroy(playerCustom.transform.GetChild(1).gameObject);
                    myAvatar.transform.SetParent(playerCustom.gameObject.transform, false);
                    Destroy(playerCustom.transform.GetChild(0).gameObject);
        // transform.SetParent(PlayerInfo.PI.allSpacesInGrid[positionOfAvatar].transform, false);
        // myPositionInGrid = positionOfAvatar;
                    myAvatar.GetComponent<RectTransform>().localPosition = new Vector3(avatarOffsetX, avatarOffsetY, 0);
                }
            }
        }

    }

    public void ArrangePlayersInCorrectOrder()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView internalPV = FindObjectOfType<PhotonPlayer>().GetComponent<PhotonView>();
            Debug.Log("Starting the rearrangement of players in the grid");
            internalPV.RPC("DetachEveryPlayerFromParent", RpcTarget.All);
            OrganizeAllPlayersParenting();
        }
    }

    [PunRPC]
    void DetachEveryPlayerFromParent()
    {
        foreach (GameObject spaceInGrid in PlayerInfo.PI.allSpacesInGrid)
        {
            if (spaceInGrid)
            {
                spaceInGrid.transform.DetachChildren(); //Detaching all players from their parent.
            }
        }
    }

    private void OrganizeAllPlayersParenting()
    {
        for(int networkPlayerIndex = 0; networkPlayerIndex < PhotonNetwork.PlayerList.Length; networkPlayerIndex++)
        {
            foreach(PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
            {
                if (player.PV.Owner == PhotonNetwork.PlayerList[networkPlayerIndex])
                {
                    player.PV.RPC("MovePlayerToRealPosition", RpcTarget.All, networkPlayerIndex, player.myPositionInGrid);
                }
            }
        }
    }

    [PunRPC]
    void MovePlayerToRealPosition(int whichPlayer, int realPlayerPosition)
    {
        foreach(PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
        {
            if (player.PV.Owner == PhotonNetwork.PlayerList[whichPlayer])
            {
                player.transform.SetParent(PlayerInfo.PI.allSpacesInGrid[realPlayerPosition].transform);
                player.transform.localPosition = new Vector3 (0, 0, 0);
                player.transform.localScale = new Vector3 (1, 1, 1);
            }
        }
    }

    public void UpdateColorsOfAllPlayers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView internalPV = FindObjectOfType<PhotonPlayer>().GetComponent<PhotonView>();
            Debug.Log("Updating colors of all players");
            CheckColorsOfPlayers();            
        }
    }

    private void CheckColorsOfPlayers()
    {
        for(int networkPlayerIndex = 0; networkPlayerIndex < PhotonNetwork.PlayerList.Length; networkPlayerIndex++)
        {
            foreach(PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
            {
                if (player.PV.Owner == PhotonNetwork.PlayerList[networkPlayerIndex])
                {
                    if(player.PV.IsMine)
                    {
                        player.PV.RPC("SendNewColorToAllPlayers", RpcTarget.All, networkPlayerIndex, player.mySelectedCharacter);
                    }
                }
            }
        }
    }

    public void AddCardToHand()
    {
        cardsIHave++;
    }

    public void LoseCardsFromHand()
    {
        cardsIHave--;
    }

    public int GetNumberOfCards()
    {
        return cardsIHave;
    }

    public void UpdateNumberOfCardsInDisplay(int playerIndex,string text)
    {
        PV.RPC("RPC_UpdateText", RpcTarget.All, playerIndex, text);
    }

    [PunRPC]
    void RPC_UpdateText(int playerIndex, string text)
    {
        // foreach(PhotonPlayer playerCustom in FindObjectsOfType<PhotonPlayer>())
        // {
        //     if (PhotonNetwork.PlayerList[playerIndex] == playerCustom.GetComponent<PhotonView>().Owner)
        //     {
        //         playerCustom.GetComponent<TMP_Text>().text = text;
        //     }
        // }
    }

    public void SendCardFromHandToTable(int cardChosenIndex, int playerIndex)
    {
        PV.RPC("SendCardPlayedToAllPlayers", RpcTarget.All, cardChosenIndex, playerIndex, cardsIHave);
    }

    [PunRPC]
    void SendCardPlayedToAllPlayers(int cardChosenIndex, int playerIndex, int cardsPlayerHas)
    {
        foreach(PhotonPlayer playerCustom in FindObjectsOfType<PhotonPlayer>())
        {
            if (PhotonNetwork.PlayerList[playerIndex] == playerCustom.GetComponent<PhotonView>().Owner)
            {
                GameObject cardPlayed = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                cardPlayed.transform.SetParent(GameController.gameController.cardsInGame.transform, false);
                cardPlayed.GetComponent<Image>().sprite = playerCustom.myCards[cardChosenIndex].artwork;
                cardPlayed.transform.localScale = new Vector3(1f, 1f, 0);
                cardPlayed.GetComponent<Button>().enabled = false;
                cardPlayed.GetComponent<HoverButton>().enabled = false;
                cardPlayed.name = playerCustom.myCards[cardChosenIndex].cardNumber.ToString() + " " + playerCustom.myCards[cardChosenIndex].cardSuit.ToString();
                Debug.Log("Player the card: " + cardPlayed.name);
                if(playerCustom.myCards[cardChosenIndex].cardNumber != 8)
                {
                    if(playerCustom.myCards[cardChosenIndex].cardSuit == Card.CardSuit.Green)
                    {
                        GameController.gameController.gameBackground.sprite = GameController.gameController.greenBackgroud;
                    }
                    else if(playerCustom.myCards[cardChosenIndex].cardSuit == Card.CardSuit.Orange)
                    {
                        GameController.gameController.gameBackground.sprite = GameController.gameController.orangeBackgroud;
                    }
                    else if(playerCustom.myCards[cardChosenIndex].cardSuit == Card.CardSuit.Purple)
                    {
                        GameController.gameController.gameBackground.sprite = GameController.gameController.purpleBackgroud;
                    }
                    else if(playerCustom.myCards[cardChosenIndex].cardSuit == Card.CardSuit.Yellow)
                    {
                        GameController.gameController.gameBackground.sprite = GameController.gameController.yellowBackgroud;
                    }
                }
                else
                {
                    if(GameController.gameController.SuitChosen == 1)
                    {
                        GameController.gameController.gameBackground.sprite = GameController.gameController.orangeBackgroud;
                    }
                    else if(GameController.gameController.SuitChosen == 2)
                    {
                        GameController.gameController.gameBackground.sprite = GameController.gameController.yellowBackgroud;
                    }
                    else if(GameController.gameController.SuitChosen == 3)
                    {
                        GameController.gameController.gameBackground.sprite = GameController.gameController.greenBackgroud;
                    }
                    else if(GameController.gameController.SuitChosen == 4)
                    {
                        GameController.gameController.gameBackground.sprite = GameController.gameController.purpleBackgroud;
                    }
                }

                if(playerCustom.GetComponent<PhotonView>().IsMine)
                {
                    PV.RPC("UpdateCardsInGameList", RpcTarget.All, cardChosenIndex, playerIndex);
                    PV.RPC("SendRemoveCardOrder", RpcTarget.All, cardChosenIndex, playerIndex);
                    StartCoroutine(ReorganizeCards(cardChosenIndex, playerCustom));
                }
                playerCustom.UpdateNumberOfCardsInDisplay(playerIndex, cardsPlayerHas.ToString());
                if(cardsPlayerHas <= 0)
                {
                    playerCustom.IWon = true;
                    foreach(PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
                    {
                        if (player.PV.Owner == PhotonNetwork.PlayerList[playerIndex])
                        {
                            if(player.PV.IsMine)
                            {
                                player.PV.RPC("UpdateGameAvatarIndex", RpcTarget.All, player.mySelectedCharacter);
                            }
                        }
                    }
                    RoomController.room.ShowWhoWon();
                    PV.RPC("UpdateGameWinnerNickName", RpcTarget.All, PhotonNetwork.PlayerList[playerIndex].NickName, playerIndex);
                }
            }
        }
    }

    [PunRPC]
    void UpdateGameWinnerNickName(string nickName, int indexOfWinner)
    {
        RoomController.room.nickNameOfWinner = nickName;
        RoomController.room.indexOfWinner = indexOfWinner;
    }

    [PunRPC]
    void UpdateGameAvatarIndex(int indexOfWinner)
    {
        RoomController.room.indexOAvatarWinner = indexOfWinner;
    }

    [PunRPC]
    void UpdateCardsInGameList(int cardChosenIndex, int playerIndex)
    {
        foreach(PhotonPlayer playerCustom in FindObjectsOfType<PhotonPlayer>())
        {
            if (PhotonNetwork.PlayerList[playerIndex] == playerCustom.GetComponent<PhotonView>().Owner)
            {
                GameController.gameController.cardsInGameList.Add(playerCustom.myCards[cardChosenIndex]);
            }
        }
    }

    [PunRPC]
    void SendRemoveCardOrder(int cardChosenIndex, int playerIndex)
    {
        foreach(PhotonPlayer playerCustom in FindObjectsOfType<PhotonPlayer>())
        {
            if (PhotonNetwork.PlayerList[playerIndex] == playerCustom.GetComponent<PhotonView>().Owner)
            {
                playerCustom.myCards.RemoveAt(cardChosenIndex);
            }
        }
    }

    IEnumerator ReorganizeCards(int cardChosenIndex, PhotonPlayer playerCustom)
    {
        Destroy(GameController.gameController.myCards.transform.GetChild(cardChosenIndex).gameObject);
        yield return new WaitForEndOfFrame();
        int childIndex = 0;
        foreach (Transform child in GameController.gameController.myCards.transform)
        {
            if (childIndex < CardDisplay.cardDisplayInstance.maxCardsPerRow)
            {
                child.gameObject.SetActive(true);
                child.localPosition = new Vector3(-((CardDisplay.cardDisplayInstance.maxCardsPerRow / 2 - childIndex) * CardDisplay.cardDisplayInstance.distanceBetweenCardsX), 0, 0);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
            childIndex++;
        }
    }
}
