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
    [SerializeField] int playerOffsetY = -84;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        currentSC = this;
    }

    void Start()
    {
        // foreach(Seat seat in seats)
        // {
        //     seat.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        // }
    }
    public void ChoseASeatAndSeat()
    {
        // foreach (PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
        // {
        //     player.transform.GetChild(0).gameObject.SetActive(false);
        // }
        StartCoroutine(HandleSeatsCoroutine());
    }

    IEnumerator HandleSeatsCoroutine()
    {
        print("Número de asientos: " + seats.Length);
        Debug.Log("Número de jugadores: " + PhotonNetwork.CurrentRoom.PlayerCount);    
        if(PhotonNetwork.CurrentRoom.PlayerCount % 2 == 0)
        {
            yield return new WaitForSeconds(1f);  
            if (PhotonNetwork.IsMasterClient)
            {
                int initial = (seats.Length - PhotonNetwork.CurrentRoom.PlayerCount)/2;
                int final = initial + PhotonNetwork.CurrentRoom.PlayerCount - 1;
                int seatChosen = Random.Range(initial, final);

                for (int playerIndex = 0; playerIndex < PhotonNetwork.PlayerList.Length; playerIndex++)
                {                    
                    foreach (PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
                    {
                        if (player.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[playerIndex])
                        {
                            if(playerIndex > 0)
                            {
                                seatChosen++;
                                if(seatChosen > final)
                                {
                                    seatChosen = initial;
                                }
                            }
                            Debug.Log("El jugador " + PhotonNetwork.PlayerList[playerIndex].NickName + " ocupará el sitio " + seatChosen);
                            PV.RPC("SendSeatPosition", RpcTarget.All, seatChosen, playerIndex);
                        }
                    }
                }
            }
        }
        else
        {
            foreach(Seat seat in FindObjectsOfType<Seat>())
            {
                seat.transform.localPosition = seat.transform.localPosition + new Vector3(seat.GetComponent<RectTransform>().rect.width/2, 0, 0);
            }
            yield return new WaitForSeconds(1f);  
            if (PhotonNetwork.IsMasterClient)
            {
                int initial = (seats.Length - PhotonNetwork.CurrentRoom.PlayerCount - 1)/2;
                int final = initial + PhotonNetwork.CurrentRoom.PlayerCount - 1;
                int seatChosen = Random.Range(initial, final);

                for (int playerIndex = 0; playerIndex < PhotonNetwork.PlayerList.Length; playerIndex++)
                {                    
                    foreach (PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
                    {
                        if (player.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[playerIndex])
                        {
                            if(playerIndex > 0)
                            {
                                seatChosen++;
                                if(seatChosen > final)
                                {
                                    seatChosen = initial;
                                }
                            }
                            Debug.Log("El jugador " + PhotonNetwork.PlayerList[playerIndex].NickName + " ocupará el sitio " + seatChosen);
                            PV.RPC("SendSeatPosition", RpcTarget.All, seatChosen, playerIndex);
                        }
                    }
                }
            }

        }
    }

    [PunRPC]
    void SendSeatPosition(int seatChosenSent, int playerOwner)
    {
        foreach(PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
        {
            CharacterType characterType = player.transform.GetChild(0).GetComponent<CharacterType>();
            characterType.ChangeImageInButtonAvatar(characterType.spriteInGame);
            if(player.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[playerOwner])
            {
                player.myRealSeat = seatChosenSent;
                SeatsController seatsController = FindObjectOfType<SeatsController>();
                player.transform.SetParent(seatsController.seats[seatChosenSent].transform);
                player.GetComponent<RectTransform>().localPosition = new Vector3(0, playerOffsetY, 0);
                player.transform.localScale = new Vector3(1, 1, 1);
                player.transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);
                if(player.GetComponent<PhotonView>().IsMine)
                {
                    
                    GameController.gameController.mySeat = seatChosenSent;
                }
                seatsController.seats[seatChosenSent].disconnectedPlayer = player.transform.GetChild(0).GetComponent<CharacterType>().spriteDisconnected;
            }
        }
        seats[seatChosenSent].seatTaken = true;
    }

    public void ShowEveryoneWhosTurnItIs(int seatPosition)
    {
        int numberOfWinners = 0;
        foreach(PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
        {
            if(player.IWon)
            {
                numberOfWinners++;
            }
        }
        
        if(numberOfWinners<=0)
        {
            PV.RPC("ToggleTurnIndicator", RpcTarget.All, seatPosition);
        }
    }

    [PunRPC]
    void ToggleTurnIndicator(int seatPosition)
    {
        Debug.Log("seatPosition = " + seatPosition);
        foreach(Seat seat in seats)
        {
            if(seat.gameObject.transform.childCount > 0)
            {
                CharacterType characterType_int = seat.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<CharacterType>();
                characterType_int.ChangeImageInButtonAvatar(characterType_int.spriteInGame);
            }

        }
        if(seats[seatPosition].gameObject.transform.childCount >0)
        {
            CharacterType characterType = seats[seatPosition].gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<CharacterType>();
            characterType.ChangeImageInButtonAvatar(characterType.spriteTurn);
            seats[seatPosition].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

}
