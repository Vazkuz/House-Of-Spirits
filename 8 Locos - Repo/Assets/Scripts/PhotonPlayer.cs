using System.Collections;
using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PhotonPlayer : MonoBehaviour
{
    [SerializeField] float avatarOffsetX;
    [SerializeField] float avatarOffsetY;
    public GameObject[] allCharacters;
    private PhotonView PV;
    public GameObject myAvatar;
    public int mySelectedCharacter;
    public int myPositionInGrid;
    public string myNickName;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        if(PV.IsMine)
        {        
            //Check if player has selected a color before. If not, default color will be white (identified with number 0)
            if (PlayerPrefs.HasKey("MY_CHARACTER"))
            {
                GetComponent<PhotonPlayer>().mySelectedCharacter = PlayerPrefs.GetInt("MY_CHARACTER");
            }
            else
            {
                GetComponent<PhotonPlayer>().mySelectedCharacter = 0;
                PlayerPrefs.SetInt("MY_CHARACTER", GetComponent<PhotonPlayer>().mySelectedCharacter);
            }
            PV.RPC("RPC_InstantiateAvatar", RpcTarget.AllBuffered, PlayerInfo.PI.mySpaceInGrid, GetComponent<PhotonPlayer>().mySelectedCharacter);

            //Check if player has entered a nickname before. If not, default name will be "Player x", where x is the position of the player in grid
            if (PlayerPrefs.HasKey("MY_NICKNAME"))
            {
                myNickName = PlayerPrefs.GetString("MY_NICKNAME");
            }
            else
            {
                myNickName = "Player " + (PlayerInfo.PI.mySpaceInGrid+1).ToString();
            }
        }
        
    }

    [PunRPC]
    void RPC_InstantiateAvatar(int positionOfAvatar, int mySelectedCharacter)
    {
        StartCoroutine(InstantiateWithLag(positionOfAvatar, mySelectedCharacter));
    }

    IEnumerator InstantiateWithLag(int positionOfAvatar, int mySelectedCharacter)
    {
        yield return new WaitForSeconds(1.5f);
        GameObject myAvatar = Instantiate(allCharacters[mySelectedCharacter], transform.position, Quaternion.identity) as GameObject;
        myAvatar.transform.localScale = new Vector3(0.6f,0.7f,0f);
        myAvatar.transform.SetParent(transform, false);
        transform.SetParent(PlayerInfo.PI.allSpacesInGrid[positionOfAvatar].transform, false);
        myPositionInGrid = positionOfAvatar;
        myAvatar.GetComponent<RectTransform>().localPosition = new Vector3(avatarOffsetX, avatarOffsetY, 0);
    }

    public void ChangePlayerAvatar(int whichCharacter)
    {        
        if (PlayerInfo.PI != null)
        {
            GetComponent<PhotonPlayer>().mySelectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MY_CHARACTER", whichCharacter);
            Photon.Realtime.Player[] playersInRoom = PhotonNetwork.PlayerList;
            PhotonPlayer[] playersInRoomCustom = FindObjectsOfType<PhotonPlayer>();
            // PhotonPlayer playerCustom;
            // foreach(PhotonPlayer player in playersInRoom)

            //Primero buscamos entre todos los PV cuál es el "mío".
            foreach (PhotonPlayer playerInRoomCustom in playersInRoomCustom)
            {
                //Cuando encontramos el "mío". Buscaremos su índice en la Lista de Jugadores de Photon.
                if(playerInRoomCustom.PV.IsMine)
                {
                    //Empezamos a buscar entre los jugadores de la lista de photon cuál es el índex "nuestro".
                    for(int playerIndex = 0; playerIndex < playersInRoom.Length; playerIndex++)
                    {
                        if (playerInRoomCustom.PV.Owner == playersInRoom[playerIndex]) //Este es el índex! Con él podemos identificar unívocamente al jugador sin confusiones
                        {
                            playerInRoomCustom.mySelectedCharacter = whichCharacter;
                            playerInRoomCustom.PV.RPC("SendNewColorToAllPlayers",RpcTarget.AllBuffered,playerIndex, GetComponent<PhotonPlayer>().mySelectedCharacter);
                        }                        
                    }
                    // playerInRoomCustom.PV.RPC("SendNewColorToAllPlayers",RpcTarget.AllBuffered,playerIndex, mySelectedCharacter);
                }
            }

        }
    }

    [PunRPC]
    private void SendNewColorToAllPlayers(int playerIndex, int mySelectedCharacter)
    {
        Debug.Log("Changing avatar color");
        //Aquí tendremos que volver a buscar el PhotonNetwork Player del jugador D:
        foreach(PhotonPlayer playerCustom in FindObjectsOfType<PhotonPlayer>())
        {
            //Cuando encontremos al owner, lo usamos para instanciar todo
            
            if (PhotonNetwork.PlayerList[playerIndex] == playerCustom.PV.Owner)
            {
                GameObject myAvatar = Instantiate(allCharacters[mySelectedCharacter], new Vector3(avatarOffsetX, avatarOffsetY, 0), Quaternion.identity);
                myAvatar.transform.localScale = new Vector3(0.6f,0.7f,0f);
                if(playerCustom.transform.childCount > 2) Destroy(playerCustom.transform.GetChild(1).gameObject);
                myAvatar.transform.SetParent(playerCustom.gameObject.transform, false);
                AvatarPreviewController avatarPreviewController = FindObjectOfType<AvatarPreviewController>();
                if (avatarPreviewController!=null) avatarPreviewController.ChangePreviewColor(myAvatar.GetComponent<Image>().color);
                Destroy(playerCustom.transform.GetChild(0).gameObject);
            }
        }        
    }

    public void ArrangePlayersInCorrectOrder()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView internalPV = FindObjectOfType<PhotonPlayer>().GetComponent<PhotonView>();
            Debug.Log("Starting the rearrangement of players in the grid");
            internalPV.RPC("DetachEveryPlayerFromParent", RpcTarget.All);
            OrganizeAllPlayersParenting();
        }
    }

    [PunRPC]
    void DetachEveryPlayerFromParent()
    {
        foreach (GameObject spaceInGrid in PlayerInfo.PI.allSpacesInGrid)
        {
            spaceInGrid.transform.DetachChildren(); //Detaching all players from their parent.
        }
    }

    private void OrganizeAllPlayersParenting()
    {
        for(int networkPlayerIndex = 0; networkPlayerIndex < PhotonNetwork.PlayerList.Length; networkPlayerIndex++)
        {
            foreach(PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
            {
                if (player.PV.Owner == PhotonNetwork.PlayerList[networkPlayerIndex])
                {
                    player.PV.RPC("MovePlayerToRealPosition", RpcTarget.All, networkPlayerIndex, player.myPositionInGrid);
                }
            }
        }
    }

    [PunRPC]
    void MovePlayerToRealPosition(int whichPlayer, int realPlayerPosition)
    {
        foreach(PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
        {
            if (player.PV.Owner == PhotonNetwork.PlayerList[whichPlayer])
            {
                player.transform.SetParent(PlayerInfo.PI.allSpacesInGrid[realPlayerPosition].transform);
                player.transform.localPosition = new Vector3 (0, 0, 0);
                player.transform.localScale = new Vector3 (1, 1, 1);
            }
        }
    }

    public void UpdateColorsOfAllPlayers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView internalPV = FindObjectOfType<PhotonPlayer>().GetComponent<PhotonView>();
            Debug.Log("Updating colors of all players");
            CheckColorsOfPlayers();            
        }
    }

    private void CheckColorsOfPlayers()
    {
        for(int networkPlayerIndex = 0; networkPlayerIndex < PhotonNetwork.PlayerList.Length; networkPlayerIndex++)
        {
            foreach(PhotonPlayer player in FindObjectsOfType<PhotonPlayer>())
            {
                if (player.PV.Owner == PhotonNetwork.PlayerList[networkPlayerIndex])
                {
                    if(player.PV.IsMine)
                    {
                        player.PV.RPC("SendNewColorToAllPlayers", RpcTarget.All, networkPlayerIndex, player.mySelectedCharacter);
                    }
                }
            }
        }
    }

}
