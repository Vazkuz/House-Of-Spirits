using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CharacterType : MonoBehaviour
{
    public int characterIndex;
    public string characterName;
    public Sprite spriteInRoom;
    public Sprite spriteInGame;
    public Sprite spriteDisconnected;
    public Sprite spriteTurn;
    public Sprite notSelected;
    public Sprite selectedByOther;
    public Sprite selectedByMe;
    // [SerializeField] GameObject playerUIIdentifier;
    // [SerializeField] float coinScaleX = 1.2f;
    // [SerializeField] float coinScaleY = 1f;
    // [SerializeField] float coinPosX = 163f;
    // [SerializeField] float coinPosY = -110f;


    // Start is called before the first frame update
    void Start()
    {
        // if(transform.parent.GetComponent<PhotonPlayer>())
        // {
        //     if(transform.parent.GetComponent<PhotonView>().IsMine)
        //     {
        //         // GameObject coinID = Instantiate(playerUIIdentifier, transform);
        //         // coinID.transform.SetParent(transform, true);
        //         // coinID.transform.localScale = new Vector3(coinScaleX, coinScaleY, 1);
        //         // coinID.transform.localPosition = new Vector3(coinPosX, coinPosY, coinID.transform.localPosition.z);
        //     }
        // }
    }

    public void ChangeImageInButtonAvatar(Sprite whichTypeOfSelection)
    {
        this.GetComponent<Image>().sprite = whichTypeOfSelection;
    }

}
