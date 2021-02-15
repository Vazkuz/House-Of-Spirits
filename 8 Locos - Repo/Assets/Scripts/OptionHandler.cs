using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionHandler : MonoBehaviour
{
    public GameObject optionCanvas;
    
    void Start()
    {
        optionCanvas.SetActive(false);
    }
    
    public void OpenOptions()
    {
        optionCanvas.SetActive(true);
    }

    public void CloseOptions()
    {
        optionCanvas.SetActive(false);
    }
}
