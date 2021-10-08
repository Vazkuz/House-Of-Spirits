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
        int intCharacterColor = PlayerPrefs.GetInt("MY_CHARACTER");
        PhotonPlayer exampleOfPlayer = FindObjectOfType<PhotonPlayer>();
        Color avatarPreviewImageColor = exampleOfPlayer.allCharacters[intCharacterColor].GetComponent<Image>().color;

        avatarPreviewImage = GetComponent<Image>();
        avatarPreviewImage.color = avatarPreviewImageColor;
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
