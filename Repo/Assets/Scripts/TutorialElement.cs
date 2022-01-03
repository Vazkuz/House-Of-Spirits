using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialElement : MonoBehaviour
{
    public bool lightController = false;
    public TMP_Text scriptText;
    public Vector3[] lightPositions;
    public string[] scripts;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(lightPositions.Length != scripts.Length)
        {
            Debug.LogError("The light positions and scripts must be arrays of the same size.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
