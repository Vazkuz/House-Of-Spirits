using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroAnimController : MonoBehaviour
{
    [SerializeField] Canvas mainCanvas;
    [SerializeField] float waitTimeToOpenCanvas = 1f;
    [SerializeField] GameObject loadingPrefab;

    [SerializeField] string folderName = "Videos";
    [SerializeField] string fileName = "";
    [SerializeField] string fileFormat = ".mp4";
    VideoPlayer videoPlayer;

    void Start()
    {
        if(MultiplayerSettings.multiplayerSettings.firstTime)
        {
            mainCanvas.enabled = false;
            videoPlayer = this.GetComponent<VideoPlayer>();
            // loadingPrefab.SetActive(false);
            videoPlayer.source = VideoSource.Url;
            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, folderName + System.IO.Path.DirectorySeparatorChar + fileName + fileFormat);
            videoPlayer.url = filePath;
            videoPlayer.loopPointReached += EndReached;

            MultiplayerSettings.multiplayerSettings.firstTime = false;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            StopIntro();
            // loadingPrefab.SetActive(true);
        }
    }

    private void StopIntro()
    {
        // videoPlayer.Stop();
        videoPlayer.targetCamera = null;
        videoPlayer.gameObject.SetActive(false);
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
        StopIntro();
        // mainCanvas.enabled = true;
    }
}
