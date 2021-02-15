﻿using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;
    public bool hasStartedLeaving;
    PhotonView PV;
    [SerializeField] TMP_Text roomNameTMP;
    [SerializeField] TMP_Text roomPasswordTMP;

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
        Debug.Log("Problema aquí 1.");
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
        Debug.Log("Problema aquí 2.");
        hasStartedLeaving = false;
    }

    public void StartGame()
    {
        //Only the master client can start the game
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting game");
        }
    }
}