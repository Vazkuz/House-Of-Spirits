using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionHandler : MonoBehaviour
{
    public GameObject optionCanvas;
    [SerializeField] GameObject optionOpener;
    
    void Start()
    {
        optionCanvas.SetActive(false);
        optionOpener.SetActive(true);
    }
    
    public void OpenOptions()
    {
        optionCanvas.SetActive(true);
        optionOpener.SetActive(false);
        foreach(GameObject nicknameShow in GameObject.FindGameObjectsWithTag("NicknameInput"))
        {
            nicknameShow.GetComponent<NicknameInputController>().enabled = true;
        }
    }

    public void CloseOptions()
    {
        optionCanvas.SetActive(false);
        optionOpener.SetActive(true);
        foreach(GameObject nicknameShow in GameObject.FindGameObjectsWithTag("NicknameInput"))
        {
            nicknameShow.GetComponent<NicknameInputController>().enabled = false;
        }
    }
}
