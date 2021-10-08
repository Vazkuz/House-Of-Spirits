using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class KickPlayerButton : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject gridToKickPlayer;
    // Start is called before the first frame update
    public void KickPlayer()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                if(gridToKickPlayer.transform.childCount > 0)
                {
                    if(gridToKickPlayer.transform.GetChild(0).GetComponent<PhotonView>().Owner == player && !player.IsLocal)
                    {
                        PhotonNetwork.CloseConnection(player);
                    }
                }
            }
        }
    }
}
