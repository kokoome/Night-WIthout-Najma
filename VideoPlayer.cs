using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlater : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName;

    void Start()
    {
        // حضري الفيديو (ضروري للـ Build)
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        videoPlayer.Play();
        // نضيف Backup في Update للـ Build في حال حدث مشكلة
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        // Backup safety check
        if (videoPlayer.isPrepared && !videoPlayer.isPlaying && videoPlayer.frame > 0)
        {
            LoadNextScene();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}