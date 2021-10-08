using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LobbyController : MonoBehaviourPunCallbacks
{
    public static LobbyController lobby;
    public string passwordAttempt;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField roomPasswordInputField;
    [SerializeField] Button createButton;
    [SerializeField] Button joinButton;
    [SerializeField] Button backButton;
    [SerializeField] Button quitButton;
    [SerializeField] TMP_Text connectingToServersText;
    [SerializeField] TMP_Text failedToJoinRoom;
    [SerializeField] float failedRoomConnectionTextTime = 2f;
    Button[] buttons;

    private void Awake() 
    {
        lobby = this;
    }

    void Start()
    {
        // Connection to Photon 
        PhotonNetwork.ConnectUsingSettings();

        //We'll need all buttons from Menu
        buttons = FindObjectsOfType<Button>();

        //Just in case, we deactivate all non necessary buttons from the Menu
        connectingToServersText.gameObject.SetActive(true);
        failedToJoinRoom.gameObject.SetActive(false);
        roomPasswordInputField.gameObject.SetActive(false);
        roomNameInputField.gameObject.SetActive(false);

        // We need to disable all buttons until we are connected to the servers.
        foreach(Button button in buttons)
        {
            GameObject buttonGameObject = button.gameObject;
            buttonGameObject.SetActive(false);
        }
        quitButton.gameObject.SetActive(true);
    }

    //First things first. We need to connect the instance of the game to the Photon Cloud. 
    //When we finished doing that, we call this callback and activate the room buttons.
    public override void OnConnectedToMaster()
    {
        Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server.");
        ShowMainMenuButtons();
        PhotonNetwork.AutomaticallySyncScene = true;

        //We also deactivate the "Connecting to servers" text.
        connectingToServersText.gameObject.SetActive(false);
    }

    //When the player pressses the Create room button, the game sets the Input Field required to enter the password the player wants the room to have.
    //It is also needed to disable all other UI elements.
    public void ActivateRoomCreation()
    {
        foreach(Button button in buttons)
        {
            GameObject buttonGameObject = button.gameObject;
            buttonGameObject.SetActive(false);
        }
        roomPasswordInputField.gameObject.SetActive(true);
        createButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    //This public method will be called by the Creation button below the Input Field that asks the player to write a password.
    //It is important to say that the player can also create the room without any password.
    public void CreateRoom()
    {
        //The name of the room is taken by concatenating a random capital letter and 3 random digits.
        string roomName = (char)('A'+Random.Range(0, 26))+Random.Range(100,1000).ToString();
        PhotonNetwork.CreateRoom(roomName);// PhotonNetwork.CreateRoom(roomName);
    }

    //Retrying to create the room in case of randomly chosing a room name already taken.
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
        CreateRoom();
    }

    //When the player presses the Join room button, the game sets the Input Fields required to enter name and password of the room to join.
    //It is also needed to disable all other UI elements.
    public void ActivateRoomJoin()
    {
        foreach(Button button in buttons)
        {
            GameObject buttonGameObject = button.gameObject;
            buttonGameObject.SetActive(false);
        }
        roomPasswordInputField.gameObject.SetActive(true);
        roomNameInputField.gameObject.SetActive(true);
        joinButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    //This public method will be called by the Joinning button below the Input Fields of name and password.
    public void JoinRoomRequest()
    {
        passwordAttempt = roomPasswordInputField.GetComponent<InputFieldController>().inputText;
        string roomName = roomNameInputField.GetComponent<InputFieldController>().inputText;
        PhotonNetwork.JoinRoom(roomName);
    }

    //Just a method to activate main menu buttons while deactivating the rest of the UI elements.
    public void ShowMainMenuButtons()
    {
        foreach(Button button in buttons)
        {
            GameObject buttonGameObject = button.gameObject;
            buttonGameObject.SetActive(true);
        }
        roomPasswordInputField.gameObject.SetActive(false);
        roomNameInputField.gameObject.SetActive(false);
        joinButton.gameObject.SetActive(false);
        createButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }    
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Couldn't connect to the room with the given name.");
        StartCoroutine(JoinRoomFailMessage());
    }

    IEnumerator JoinRoomFailMessage()
    {
        failedToJoinRoom.gameObject.SetActive(true);
        yield return new WaitForSeconds(failedRoomConnectionTextTime);
        failedToJoinRoom.gameObject.SetActive(false);
    }
}
