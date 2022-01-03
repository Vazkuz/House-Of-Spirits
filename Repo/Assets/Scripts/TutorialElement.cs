using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialElement : MonoBehaviour
{
    public bool lightController = false;
    public bool scriptPosController = false;
    public TMP_Text scriptText;
    public Vector3[] lightPositions;
    public string[] scripts;
    public Vector3[] scriptPositions;

    [Header("Light Controlling")]
    [Range(0,2)] public float globalLightIntensity = 0.3f;
    [Range(0,2)] public float pointLightIntensity = 0.8f;
    [Range(0,5)] public float pointLightInnerRadius = 1.9f;
    [Range(0,5)] public float pointLightOuterRadius = 2f;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(lightPositions.Length != scripts.Length)
        {
            Debug.LogError("The light positions and scripts must be arrays of the same size.");
        }
        else if(scriptPosController && (lightPositions.Length != scriptPositions.Length))
        {
            Debug.LogError("The script and light positions must be arrays of the same size.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
