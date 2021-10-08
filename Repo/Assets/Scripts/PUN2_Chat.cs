﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PUN2_Chat : MonoBehaviourPun
{
    bool isChatting = false;
    string chatInput = "";

    [System.Serializable]
    public class ChatMessage
    {
        public string sender = "";
        public string message = "";
        public float timer = 0;
    }

    List<ChatMessage> chatMessages = new List<ChatMessage>();

    // Start is called before the first frame update
    void Start()
    {
        //Initialize Photon View
        if(gameObject.GetComponent<PhotonView>() == null)
        {
            PhotonView photonView = gameObject.AddComponent<PhotonView>();
            photonView.ViewID = 1;
        }
        else
        {
            photonView.ViewID = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.T) && !isChatting)
        {
            isChatting = true;
            chatInput = "";
        }

        //Hide messages after timer is expired
        for (int i = 0; i < chatMessages.Count; i++)
        {
            if (chatMessages[i].timer > 0)
            {
                chatMessages[i].timer -= Time.deltaTime;
            }
        }
    }

    void OnGUI()
    {
        if (!isChatting)
        {
            GUI.color = new Color(1,0,0);
            GUI.skin.label.fontSize = 14;
            GUI.Label(new Rect(5, Screen.height - 25, 200, 25), "<b>Press 'T' to chat</b>");
        }
        else
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                isChatting = false;
                if(chatInput.Replace(" ", "") != "")
                {
                    //Send message
                    photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, chatInput);
                }
                chatInput = "";
            }

            GUI.SetNextControlName("ChatField");
            GUI.color = new Color(1,0,0);
            GUI.Label(new Rect(5, Screen.height - 25, 200, 25), "<b>Say:</b>");
            GUI.color = new Color(1,0,0);
            GUIStyle inputStyle = GUI.skin.GetStyle("box");
            inputStyle.alignment = TextAnchor.MiddleLeft;
            chatInput = GUI.TextField(new Rect(10 + 25, Screen.height - 27, 400, 22), chatInput, 60, inputStyle);

            GUI.FocusControl("ChatField");
        }
        
        //Show messages
        for(int i = 0; i < chatMessages.Count; i++)
        {
            if(chatMessages[i].timer > 0 || isChatting)
            {
                GUI.Label(new Rect(5, Screen.height - 50 - 25 * i, 500, 25), chatMessages[i].sender + ": " + chatMessages[i].message);
                GUI.color = new Color(1,0,0);
            }
        } 
    }

    [PunRPC]
    void SendChat(Player sender, string message)
    {
        ChatMessage m = new ChatMessage();
        m.sender = "<b>"+sender.NickName+"</b>";
        m.message = "<b>"+message+"</b>";
        m.timer = 10.0f;

        chatMessages.Insert(0, m);
        if(chatMessages.Count > 8)
        {
            chatMessages.RemoveAt(chatMessages.Count - 1);
        }
    }
}