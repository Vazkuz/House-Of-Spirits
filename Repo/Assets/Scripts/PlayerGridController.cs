using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class PlayerGridController : MonoBehaviour
{
    [SerializeField] TMP_Text nicknameText;
    [SerializeField] TMP_Text godnameText;

    void OnEnable() 
    {
        foreach(GameObject nicknameFrame in GameObject.FindGameObjectsWithTag("NicknameFrame"))
        {
            nicknameFrame.SetActive(false);
        }

        foreach(GameObject godnameFrame in GameObject.FindGameObjectsWithTag("GodnameFrame"))
        {
            godnameFrame.SetActive(false);
        }
    }

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
                        godnameText.text = transform.GetChild(0).GetComponent<PhotonPlayer>().mySelectedName;
                    }
                }
                else
                {
                    nicknameText.text = "";
                    godnameText.text = "";
                }
            }
            else
            {
                if(transform.childCount > 0)
                {
                    if (transform.GetChild(0).GetComponent<PhotonView>().Owner == player)
                    {
                        nicknameText.text = player.NickName;
                        godnameText.text = transform.GetChild(0).GetComponent<PhotonPlayer>().mySelectedName;
                    }
                }
                else
                {
                    nicknameText.text = "";
                    godnameText.text = "";
                }
            }
        }
    }
}
