using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    [SerializeField] Image fondo;
    bool firstTime = true;
    // Start is called before the first frame update
    void Start()
    {
        // fondo.enabled = false;
        // firstTime = false;
    }

    void OnEnable()
    {
        print("enabling");
        fondo.enabled = true;
    }

    void OnDisable()
    {
        print("disabling");
        fondo.enabled = false;
    }
}
