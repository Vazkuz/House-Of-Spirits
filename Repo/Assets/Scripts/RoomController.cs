﻿using System.Collections;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomController : MonoBehaviourPunCallbacks
{
    public static RoomController room;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField roomPasswordInputField;
    RoomOptions roomOptions;
    private PhotonView PV;
    // bool isRoomLoaded; //TO USE LATER
    public int currentScene;
    public int playersInGame;
    public string roomPassword;
    public bool isSomeonePlayingNow = false;
    public string nickNameOfWinner;
    public TMP_Text PlayerWon;
    string passwordAttemptFromClient;
    bool kickedWrongPassword = false;
    int whoLeft;

    void Awake()
    {
        if (RoomController.room == null)
        {
            RoomController.room = this;
        }
        else
        {
            if (RoomController.room != this)
            {
                Destroy(RoomController.room.gameObject);
                RoomController.room = this;
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start () 
    {
        PV = GetComponent<PhotonView>();
    }

    public override void OnEnable()
    {
        //subscribe to functions
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //called when multiplayer scene is loaded
        currentScene = scene.buildIndex;
        if(currentScene == MultiplayerSettings.multiplayerSettings.roomScene)
        {
            RPC_CreatePlayer();
        }
        if(currentScene == MultiplayerSettings.multiplayerSettings.gameScene)
        {
            FindObjectOfType<SeatsController>().ChoseASeatAndSeat();
        }
        if(currentScene == MultiplayerSettings.multiplayerSettings.gameEndedScene)
        {
            PV.RPC("RPC_ShowWhoWon", RpcTarget.All, RoomController.room.nickNameOfWinner);
        }
    }

    public void PrepareSendingPlayerSequence(bool isUpdate, bool makeNextPlayerDraw, int cardsToDraw, int playerIndex)
    {
        if(!isUpdate)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameController.gameController.currentTurn = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
                PV.RPC("SendFirstPlayerToAllPlayers", RpcTarget.All, GameController.gameController.currentTurn, false, 0);
            }
        }
        else
        {            
            PV.RPC("SendFirstPlayerToAllPlayers", RpcTarget.All, GameController.gameController.currentTurn, makeNextPlayerDraw, cardsToDraw);
        }
    }
    
    [PunRPC]
    void SendFirstPlayerToAllPlayers(int currentTurn, bool makeNextPlayerDraw, int cardsToDraw)
    {
        GameController.gameController.turnOptions.SetActive(false);
        GameController.gameController.currentTurn = currentTurn;
        foreach(PhotonPlayer photonPlayer in FindObjectsOfType<PhotonPlayer>())
        {
            if (PhotonNetwork.PlayerList[GameController.gameController.currentTurn] == photonPlayer.GetComponent<PhotonView>().Owner &&
                    photonPlayer.GetComponent<PhotonView>().IsMine)
            {
                if(makeNextPlayerDraw)
                {
                    GameController.gameController.cardsToDraw = cardsToDraw;
                }
                else
                {
                    GameController.gameController.cardsToDraw = 0;
                }
                GameController.gameController.turnOptions.SetActive(true);
                if(makeNextPlayerDraw)
                {
                    GameController.gameController.SomeoneWantsMeToDraw(makeNextPlayerDraw, cardsToDraw, photonPlayer, GameController.gameController.currentTurn);
                }
            }
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
        playersInGame++;
        Debug.Log("Actualmente hay " + playersInGame + " jugadores en la sala.");
    }

    //Once the master client has created the room, the game will go to the Room Scene. //ROOMCONTROLLER
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Room created. Room name: " + PhotonNetwork.CurrentRoom.Name);
    }

    //Once the client has joinned the room, the master client needs to validate if the password given by the player (if any) matches with the room's password. //ROOMCONTROLLER
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room. Room name: " + PhotonNetwork.CurrentRoom.Name + ". Number of players currently on room: " + PhotonNetwork.CurrentRoom.PlayerCount);

        //The password doesn't need to be validated if the client is also the master client.
        if(PhotonNetwork.IsMasterClient)
        {
            string attemptToChangePassword = roomPasswordInputField.GetComponent<InputFieldController>().inputText;
            roomOptions = new RoomOptions();
            roomOptions.CustomRoomProperties = new Hashtable();
            //If the master client choses a password, the game needs to add it to the room options (for further validations).
            if (attemptToChangePassword != null)
            {
                roomOptions.CustomRoomProperties.Add("pwd", attemptToChangePassword);
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomOptions.CustomRoomProperties);
            }
            PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.roomScene);
        }
        //Validation of passwords.
        else
        {
            PV.RPC("RPC_SendPasswordAttempt", RpcTarget.MasterClient, LobbyController.lobby.passwordAttempt);
        }
    }

    //Store password in a variable to further validations. //ROOMCONTROLLER
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        if (propertiesThatChanged.ContainsKey("pwd"))
        {
            RoomController.room.roomPassword = propertiesThatChanged["pwd"].ToString();
        }
    }

    //When a player enters the room we initialize the validation of passwords. We need a Coroutine because reading of Input Fields is kinda slow. //ROOMCONTROLLER
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has entered the room. Currently number of players: " + PhotonNetwork.CurrentRoom.PlayerCount);
        // We try using the password the client gave to "enter" the room. If the password is not the one chose by the master client, we kick the player.
        kickedWrongPassword = false;
        StartCoroutine(TryPasswordGivenByClient(newPlayer));
        if(PhotonNetwork.CurrentRoom.PlayerCount >= 8)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    //Validation of passwords. //ROOMCONTROLLER
    private IEnumerator TryPasswordGivenByClient(Player newPlayer)
    {
        //  We wait 1 second just to give the server the opportunity to receive the password and make the validations.
        yield return new WaitForSeconds(1);

        // If passwords don't match, we kick the player.
        if (RoomController.room.roomPassword != passwordAttemptFromClient && newPlayer.IsMasterClient == false)
        {
            Debug.Log("The player will be kicked (wrong password).");
            kickedWrongPassword = true;
            PhotonNetwork.CloseConnection(newPlayer);
        }
        // Else, we leave the player in the room and we load the Room Scene.
        else
        {
            Debug.Log("Player has succesfully entered the room. Now there are " + PhotonNetwork.CurrentRoom.PlayerCount + " players on the room.");
            kickedWrongPassword = false;
            StartCoroutine(WaitAndRearrange());
            StartCoroutine(WaitAndUpdateColors());
            if(PhotonNetwork.IsMasterClient)
            {
                PV.RPC("ShowPasswordToAll", RpcTarget.All, RoomController.room.roomPassword);
            }
        }
    }

    [PunRPC]
    void ShowPasswordToAll(string roomPassword)
    {
        roomOptions = new RoomOptions();
        roomOptions.CustomRoomProperties = new Hashtable();
        GameSetup.GS.roomPasswordTMP.text = "Room password: " + roomPassword;
        roomOptions.CustomRoomProperties.Add("pwd", roomPassword);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomOptions.CustomRoomProperties);
    }

    IEnumerator WaitAndRearrange()
    {
        if(currentScene == MultiplayerSettings.multiplayerSettings.roomScene)
        {
            yield return new WaitForSeconds(1.55f);
            FindObjectOfType<PhotonPlayer>().ArrangePlayersInCorrectOrder();
        }
    }

    IEnumerator WaitAndUpdateColors()
    {
        yield return new WaitForSeconds(1.95f);
        FindObjectOfType<PhotonPlayer>().UpdateColorsOfAllPlayers();
    }



    //We let the master client know that one player has left the room. Maybe we will add something later.  //ROOMCONTROLLER
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        if(currentScene == MultiplayerSettings.multiplayerSettings.roomScene)
        {
            StartCoroutine(SearchForEmptySpaceInGrid());
            Debug.Log(otherPlayer + " left the room. Number of players currently on the room: " + PhotonNetwork.CurrentRoom.PlayerCount);
            playersInGame--;
        }

        if(currentScene == MultiplayerSettings.multiplayerSettings.gameScene)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                PV.RPC("RPC_CheckIfLefterWasPlaying", RpcTarget.All);
                if(!RoomController.room.isSomeonePlayingNow)
                {
                    if(GameController.gameController.sequencePositive)
                    {
                        RoomController.room.PrepareSendingPlayerSequence(true, false, 0, GameController.gameController.currentTurn);
                    }
                    else
                    {
                        GameController.gameController.currentTurn--;
                        if(GameController.gameController.currentTurn < 0)
                        {
                            GameController.gameController.currentTurn = PhotonNetwork.PlayerList.Length;
                        }
                        RoomController.room.PrepareSendingPlayerSequence(true, false, 0, GameController.gameController.currentTurn);
                    }
                }
            }
        }
        
        if(PhotonNetwork.CurrentRoom.PlayerCount < 8)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }

    [PunRPC]
    void RPC_CheckIfLefterWasPlaying()
    {
        if(GameController.gameController.isMyTurn)
        {
            PV.RPC("RPC_IsSomeonesTurnRightNow", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    void RPC_IsSomeonesTurnRightNow()
    {
        RoomController.room.isSomeonePlayingNow = true;
    }

    IEnumerator SearchForEmptySpaceInGrid()
    {
        yield return null;//new WaitForSeconds(1f);
        //Who left?
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount + 1; i++)
        {
            if (PlayerInfo.PI.allSpacesInGrid[i].transform.childCount == 0)
            {
                whoLeft = i;
                if (!kickedWrongPassword) PV.RPC("UpdatePositionsInNetwork", RpcTarget.AllBuffered, whoLeft);
            }
        }
    }

    [PunRPC]
    void UpdatePositionsInNetwork(int whoLeft)
    {
        PlayerInfo.PI.UpdatePositionInGrid(whoLeft);
    }

    //The client sends the Master client a request to enter the room with a specific password (which will be validated afterwards).
    [PunRPC]
    void RPC_SendPasswordAttempt(string inputText)
    {
        passwordAttemptFromClient = inputText;
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        if (!GameSetup.GS.hasStartedLeaving)
        {
            StartCoroutine(GameSetup.GS.DisconnectAndLoad());
        }
    }

    public void SendSequenceOrder(bool order)
    {
        PV.RPC("RPC_SendSequenceOrder", RpcTarget.All, order);
    }

    [PunRPC]
    void RPC_SendSequenceOrder(bool order)
    {
        GameController.gameController.sequencePositive = order;
        if(GameController.gameController.sequencePositive)
        {
            GameController.gameController.directionChanged.SetActive(false);
        }
        else
        {
            GameController.gameController.directionChanged.SetActive(true);
        }
    }

    public void SendCardChosen8(bool ivePlayed8, int suitChosen)
    {
        PV.RPC("RPC_SendCardChosen8", RpcTarget.All, ivePlayed8, suitChosen);
    }

    [PunRPC]
    void RPC_SendCardChosen8(bool ivePlayed8, int suitChosen)
    {
        if(ivePlayed8)
        {
            GameController.gameController.ivePlayed8 = true;
            if (suitChosen == 1)
            {
                GameController.gameController.cardSuitChosen = Card.CardSuit.Hearts;
            }
            else if (suitChosen == 2)
            {
                GameController.gameController.cardSuitChosen = Card.CardSuit.Clovers;
            }
            else if (suitChosen == 3)
            {
                GameController.gameController.cardSuitChosen = Card.CardSuit.Tiles;
            }
            else if (suitChosen == 4)
            {
                GameController.gameController.cardSuitChosen = Card.CardSuit.Pikes;
            }
            GameController.gameController.suitChosenGo[suitChosen-1].SetActive(true);
        }
        else
        {
            GameController.gameController.ivePlayed8 = false;
            foreach(GameObject suitChosenGoElement in GameController.gameController.suitChosenGo)
            {
                suitChosenGoElement.SetActive(false);
            }
        }
    }

    public void ShowWhoWon()
    {
        GameSetup.GS.GoToGameEndedScene();
    }
    
    [PunRPC]
    void RPC_ShowWhoWon(string nickName)
    {
        RoomController.room.PlayerWon = FindObjectOfType<TMP_Text>();
        PlayerWon.text = nickName + "\nWins!";
    }

}
