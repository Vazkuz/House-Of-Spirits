using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class AvatarPreviewController : MonoBehaviour
{
    Image avatarPreviewImage;
    
    void OnEnable()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
                
            if (PhotonNetwork.LocalPlayer.IsLocal)
            {
                PhotonPlayer exampleOfPlayer = FindObjectOfType<PhotonPlayer>();
                Color avatarPreviewImageColor = exampleOfPlayer.allCharacters[i].GetComponent<Image>().color;

                avatarPreviewImage = GetComponent<Image>();
                avatarPreviewImage.color = avatarPreviewImageColor;
            }
        }
    }

    // Update is called once per frame
    public void ChangePreviewColor(Color newPreviewColor)
    {
        if(PhotonNetwork.LocalPlayer.IsLocal)
        {
            avatarPreviewImage.color = newPreviewColor;
        }
    }
}
