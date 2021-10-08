using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class PlayerGridController : MonoBehaviour
{
    [SerializeField] TMP_Text nicknameText;
    // Update is called once per frame
    void Update()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            if(player.IsLocal)
            {
                if(transform.childCount > 0)
                {
                    if (transform.GetChild(0).GetComponent<PhotonView>().Owner == player)
                    {
                        nicknameText.text = player.NickName;
                    }
                }
                else
                {
                    nicknameText.text = "";
                }
            }
            else
            {
                if(transform.childCount > 0)
                {
                    if (transform.GetChild(0).GetComponent<PhotonView>().Owner == player)
                    {
                        nicknameText.text = player.NickName;
                    }
                }
                else
                {
                    nicknameText.text = "";
                }
            }
        }
    }
}
