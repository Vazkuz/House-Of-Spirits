using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AvatarPreviewController : MonoBehaviour
{
    public static AvatarPreviewController APC;
    public string[] avatarNames;
    public CharacterType[] characterTypes;
    public int previousSelection;
    [SerializeField] TMP_Text avatarGodName;
    Image avatarPreviewImage;
    int totalAvatars;

    [SerializeField] Animator animator;
    public int currentAnimation;

    void Awake()
    {
        if (AvatarPreviewController.APC == null)
        {
            AvatarPreviewController.APC = this;
        }
        else
        {
            if (AvatarPreviewController.APC != this)
            { 
                Destroy(AvatarPreviewController.APC.gameObject);
                AvatarPreviewController.APC = this;
            }
        }
    }

    void Start()
    {
        totalAvatars = avatarNames.Length;
    }

    public void ChangePreviewAnimation(int newAvatarAnimation)
    {
        if(PhotonNetwork.LocalPlayer.IsLocal)
        {
            animator.SetInteger("avatarChosen", newAvatarAnimation);
            currentAnimation = newAvatarAnimation;
            ChangePreviewGodsName(newAvatarAnimation);
        }
    }

    public void ChangePreviewGodsName(int newAvatarAnimation)
    {
        string name = avatarNames[newAvatarAnimation];
        if(PhotonNetwork.LocalPlayer.IsLocal)
        {
            avatarGodName.text = name;
        }
    }

    public void ControlPreviewUsingArrows(bool right)
    {
        if(right)
        {
            do
            {
                currentAnimation++;
                if(currentAnimation >= totalAvatars)
                {
                    currentAnimation = 0;
                }
            }while(RoomController.room.avatarsTaken.Contains(currentAnimation) && currentAnimation != previousSelection);
        }
        else
        {
            do
            {
                currentAnimation--;
                if(currentAnimation < 0)
                {
                    currentAnimation = totalAvatars-1;
                }
            }while(RoomController.room.avatarsTaken.Contains(currentAnimation) && currentAnimation != previousSelection);
        }
        ChangePreviewAnimation(currentAnimation);
        FindObjectOfType<PhotonPlayer>().ChangePlayerAvatar(currentAnimation);
        //CharacterType[] characterTypes = FindObjectsOfType<CharacterType>();
        //characterTypes[currentAnimation].ChangeImageInButtonAvatar(characterTypes[currentAnimation].selectedByMe);
    }

}
