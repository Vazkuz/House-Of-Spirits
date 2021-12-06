using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class NicknameInputController : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] bool isPlaceholderHideOnSelect;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(player.IsLocal)
            {
                transform.GetComponent<TMP_InputField>().text = player.NickName;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(player.IsLocal)
            {
                player.NickName = transform.GetComponent<TMP_InputField>().text;
                PlayerPrefs.SetString("MY_NICKNAME", player.NickName);
            }
        }
    }

    public void OnInputFieldSelect()
    {
        if (this.isPlaceholderHideOnSelect == true)
        {
            this.inputField.placeholder.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// The input field deselect
    /// </summary>
    public void OnInputFieldDeselect()
    {
        if (this.isPlaceholderHideOnSelect == true)
        {
            this.inputField.placeholder.gameObject.SetActive(true);
        }
    }

}
