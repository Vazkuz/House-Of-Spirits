using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SeatsController : MonoBehaviour
{
    public Seat[] seats;
    public static SeatsController currentSC;
    PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        currentSC = this;
    }

    void Start()
    {
        foreach(Seat seat in seats)
        {
            seat.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public void ChoseASeatAndSeat()
    {
        foreach (PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
        {
            player.transform.GetChild(0).gameObject.SetActive(false);
        }
        StartCoroutine(HandleSeatsCoroutine());
    }

    IEnumerator HandleSeatsCoroutine()
    {
        yield return new WaitForSeconds(1f);        
        if (PhotonNetwork.IsMasterClient)
        {
            int seatChosen = Random.Range(0, seats.Length);
            for (int playerIndex = 0; playerIndex < PhotonNetwork.PlayerList.Length; playerIndex++)
            {                    
                foreach (PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
                {
                    if (player.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[playerIndex])
                    {
                        if(playerIndex > 0)
                        {
                            seatChosen++;
                            if(seatChosen >= seats.Length)
                            {
                                seatChosen = 0;
                            }
                        }
                        Debug.Log("El jugador " + PhotonNetwork.PlayerList[playerIndex].NickName + " ocupará el sitio " + seatChosen);
                        PV.RPC("SendSeatPosition", RpcTarget.All, seatChosen, playerIndex);
                        seats[seatChosen].seatTaken = true;
                    }
                }
            }
        }
        foreach (PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
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
                if(player.GetComponent<PhotonView>().IsMine)
                {
                    GameController.gameController.mySeat = seatChosenSent;
                }
            }
        }
    }

    public void ShowEveryoneWhosTurnItIs(int seatPosition)
    {
        PV.RPC("ToggleTurnIndicator", RpcTarget.All, seatPosition);
    }

    [PunRPC]
    void ToggleTurnIndicator(int seatPosition)
    {
        Debug.Log("seatPosition = " + seatPosition);
        foreach(Seat seat in seats)
        {
            seat.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        seats[seatPosition].transform.GetChild(0).gameObject.SetActive(true);
    }

}
