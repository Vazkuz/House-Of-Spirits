using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    [SerializeField] GameObject loadingBackground;
    [SerializeField] GameObject loadingAnimation;
    [SerializeField] GameObject loadingText;
    [SerializeField] float timeOfLoading = 4f;
    bool chatExists = false;
    // Start is called before the first frame update
    void Start()
    {
        loadingBackground.SetActive(true);
        loadingAnimation.SetActive(true);
        loadingText.SetActive(true);
        if(FindObjectOfType<PUN2_Chat>())
        {
            FindObjectOfType<PUN2_Chat>().enabled = false;
            chatExists = true;
        }
        StartCoroutine(StopLoading());
    }

    IEnumerator StopLoading()
    {
        yield return new WaitForSeconds(timeOfLoading);
        print("Qué fueee");
        loadingBackground.SetActive(false);
        loadingAnimation.SetActive(false);
        loadingText.SetActive(false);
        if(chatExists)
        {
            FindObjectOfType<PUN2_Chat>().enabled = true;
        }
    }

}
