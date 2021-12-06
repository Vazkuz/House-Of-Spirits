using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AvatarPreviewController : MonoBehaviour
{
    public static AvatarPreviewController APC;
    [SerializeField] TMP_Text avatarGodName;
    Image avatarPreviewImage;

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
            if (RoomController.room != this)
            { 
                Destroy(AvatarPreviewController.APC.gameObject);
                AvatarPreviewController.APC = this;
            }
        }
    }

    public void ChangePreviewAnimation(int newAvatarAnimation)
    {
        if(PhotonNetwork.LocalPlayer.IsLocal)
        {
            animator.SetInteger("avatarChosen", newAvatarAnimation);
            currentAnimation = newAvatarAnimation;
        }
    }

    public void ChangePreviewGodsName(string name)
    {
        if(PhotonNetwork.LocalPlayer.IsLocal)
        {
            avatarGodName.text = name;
        }
    }

}
