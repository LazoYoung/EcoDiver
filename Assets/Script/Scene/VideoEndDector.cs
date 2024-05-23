using System.Collections;
using UnityEngine;
using UnityEngine.Video;
namespace Script.Scene
{
    public class VideoEndDetector : MonoBehaviour
    {
        private VideoPlayer videoPlayer;

        void Start()
        {
            // Get the VideoPlayer component from the GameObject
            videoPlayer = GetComponent<VideoPlayer>();

            if (videoPlayer == null)
            {
                Debug.LogError("VideoPlayer component not found on this GameObject.");
                return;
            }

            // Register the method to the loopPointReached event
            videoPlayer.loopPointReached += OnVideoEnd;
        }

        void OnDestroy()
        {
            // Unregister the method to avoid memory leaks
            if (videoPlayer != null)
            {
                videoPlayer.loopPointReached -= OnVideoEnd;
            }
        }

        // Method to be called when the video ends
        private void OnVideoEnd(VideoPlayer vp)
        {
            //wiat 5 seconds before loading the next scene
            StartCoroutine(WaitAndLoadScene(5));

        }

        private IEnumerator WaitAndLoadScene(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            SceneLoader.Instance.LoadNextScene();
        }
    }
}
