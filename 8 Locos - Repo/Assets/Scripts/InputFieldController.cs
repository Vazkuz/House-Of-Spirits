using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class InputFieldController : MonoBehaviour
{
    public string inputText;
    TMP_InputField inputField = null;
    [SerializeField] bool onlyCapitalLetters = false;

    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        if (onlyCapitalLetters)
        {
            inputField.onValidateInput += delegate (string input, int charIndex, char addedChar) { return capitalizeLetters(addedChar);};
        }
    }

    private char capitalizeLetters(char c)
    {
        if (c >= 'a' && c <= 'z') { return (char)((int)c - 'a' + 'A');}
        else if ((c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_' || c == ' ') {return c;}
        else {return '\0';}
    }

    public void ChangeInputText(string newText)
    {
        inputText = newText;
    }

}
