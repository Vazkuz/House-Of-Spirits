using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{   
    public static PlayerInfo PI;
    public PhotonView PV;

    public int mySpaceInGrid;

    public GameObject[] allSpacesInGrid;
    public GameObject[] nicknameFrames;
    public GameObject[] godnameFrames;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }
    void OnEnable()
    {
        if (PlayerInfo.PI == null)
        {
            PlayerInfo.PI = this;
        }
        else
        {
            if (PlayerInfo.PI != this)
            {
                Destroy(PlayerInfo.PI.gameObject);
                PlayerInfo.PI = this;
            }
        }
        // DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Awake()
    {
        mySpaceInGrid = PhotonNetwork.CurrentRoom.PlayerCount-1;
        PlayerPrefs.SetInt("MY_SPACE_IN_GRID", mySpaceInGrid);
    }

    public void UpdatePositionInGrid(int newPos)
    {
        PlayerPrefs.SetInt("MY_SPACE_IN_GRID", newPos);
        for(int gridPositions = newPos+1; gridPositions < allSpacesInGrid.Length; gridPositions++)
        {
            if(allSpacesInGrid[gridPositions].transform.childCount > 0)
            {
                allSpacesInGrid[gridPositions].transform.GetChild(0).SetParent(allSpacesInGrid[gridPositions-1].transform, false);
                if(allSpacesInGrid[gridPositions-1].transform.GetChild(0).GetComponent<PhotonPlayer>())
                {
                    allSpacesInGrid[gridPositions-1].transform.GetChild(0).GetComponent<PhotonPlayer>().myPositionInGrid = gridPositions-1;
                }
            }
        }

        foreach(GameObject nicknameFrame in GameObject.FindGameObjectsWithTag("NicknameFrame"))
        {
            nicknameFrame.SetActive(false);
        }

        for(int playerIndex = 0; playerIndex < PhotonNetwork.CurrentRoom.PlayerCount; playerIndex++)
        {
            nicknameFrames[playerIndex].SetActive(true);
        }
        

        foreach(GameObject godnameFrame in GameObject.FindGameObjectsWithTag("GodnameFrame"))
        {
            godnameFrame.SetActive(false);
        }

        for(int playerIndex = 0; playerIndex < PhotonNetwork.CurrentRoom.PlayerCount; playerIndex++)
        {
            godnameFrames[playerIndex].SetActive(true);
        }

    }

}
