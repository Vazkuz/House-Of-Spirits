using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    ChatClient chatClient;
    string userId;
    [SerializeField] GameObject chatPanel;
    [SerializeField] GameObject openChatButton;
    [SerializeField] GameObject closeChatButton;

    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        userId = PlayerPrefs.GetString("MY_NICKNAME");
        chatClient = new ChatClient(this);
        closeChatButton.SetActive(false);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(userId));
        chatPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        chatClient.Service();
    }

    public void OpenChatViewer(bool isOpen)
    {
        chatPanel.SetActive(isOpen);
        openChatButton.SetActive(!isOpen);
        closeChatButton.SetActive(isOpen);
    }

    public void JoinChat()
    {
        
    }
}
