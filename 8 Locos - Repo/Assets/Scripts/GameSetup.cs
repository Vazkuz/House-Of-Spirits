using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;
    public bool hasStartedLeaving;
    PhotonView PV;
    public TMP_Text roomNameTMP;
    public TMP_Text roomPasswordTMP;

    public Transform[] spawnPoints;

    IEnumerator Start() 
    {
        PV = GetComponent<PhotonView>();
        roomNameTMP.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
        yield return new WaitForSeconds(0.5f);
        roomPasswordTMP.text = "Room password: " + RoomController.room.roomPassword;
    }

    void OnEnable() 
    {
        if (GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }

    public void DisconnectFromRoom()
    {
        hasStartedLeaving = true;
        StartCoroutine(DisconnectAndLoad());
    }

    public IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while(PhotonNetwork.IsConnected) yield return null;
        Destroy(RoomController.room.gameObject);
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
        hasStartedLeaving = false;
    }

    public void StartGame()
    {
        //Only the master client can start the game
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting game");
            SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.gameScene);
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }
}