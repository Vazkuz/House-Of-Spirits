using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroAnimController : MonoBehaviour
{
    [SerializeField] Canvas mainCanvas;
    [SerializeField] float waitTimeToOpenCanvas = 1f;
    [SerializeField] GameObject loadingPrefab;
    VideoPlayer videoPlayer;
    void Awake()
    {
        mainCanvas.enabled = false;
        videoPlayer = this.GetComponent<VideoPlayer>();
    }

    void Start()
    {
        loadingPrefab.SetActive(false);
        videoPlayer.loopPointReached += EndReached;
    }

    void Update()
    {
        if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            StopIntro();
        }
    }

    private void StopIntro()
    {
        videoPlayer.Stop();
        loadingPrefab.SetActive(true);
        mainCanvas.enabled = true;
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        print("Terminó la intro");
        StartCoroutine(WaitAndEnableCanvas());
    }

    IEnumerator WaitAndEnableCanvas()
    {
        yield return new WaitForSeconds(waitTimeToOpenCanvas);
        videoPlayer.Stop();
        mainCanvas.enabled = true;
    }
}
