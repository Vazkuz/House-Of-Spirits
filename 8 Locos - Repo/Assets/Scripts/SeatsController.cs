using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SeatsController : MonoBehaviour
{
    public Seat[] seats;
    PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public void ChoseASeatAndSeat()
    {
        foreach(PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
        {
            player.transform.GetChild(0).gameObject.SetActive(false);
        }
        if(PhotonNetwork.IsMasterClient)
        {
            foreach(PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
            {
                for(int playerIndex = 0; playerIndex < PhotonNetwork.PlayerList.Length; playerIndex++)
                {
                    if(player.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[playerIndex])
                    {
                        int seatChosen = Random.Range(0, seats.Length);
                        while (seats[seatChosen].seatTaken)
                        {
                            seatChosen = Random.Range(0, seats.Length);
                        }
                        Debug.Log("El jugador " + PhotonNetwork.PlayerList[playerIndex].NickName + " ocupará el sitio " + seatChosen);
                        PV.RPC("SendSeatPosition", RpcTarget.All, seatChosen, playerIndex);
                        seats[seatChosen].seatTaken = true;
                    }
                }
            }
        }
        foreach(PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
        {
            player.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    

    [PunRPC]
    void SendSeatPosition(int seatChosenSent, int playerOwner)
    {
        foreach(PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
        {
            if(player.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[playerOwner])
            {
                SeatsController seatsController = FindObjectOfType<SeatsController>();
                player.transform.SetParent(seatsController.seats[seatChosenSent].transform);
                player.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}
