using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Scene
{
    public class FadeScreen : MonoBehaviour
    {
        [SerializeField] [Tooltip("Fade on start")]
        private bool fadeOnStart = true;
        [SerializeField] [Tooltip("Fade duration in seconds")]
        private float fadeDuration = 2;
        public float FadeDuration
        {
            get => fadeDuration;
        }
        private Color fadeColor;
        private Renderer rend;

        // Start is called before the first frame update
        void Start()
        {
            rend = GetComponent<Renderer>();
            if (fadeOnStart)
            {
                FadeIn();
            }
        }

        public void FadeIn()
        {
            Fade(1, 0);
        }

        public void FadeOut()
        {
            Fade(0, 1);
        }

        public void Fade(float alphaIn, float alphaOut)
        {
            Debug.Log("Fade from " + alphaIn + " to " + alphaOut);
            StartCoroutine(FadeRoutine(alphaIn, alphaOut));
        }

        private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
        {
            float time = 0;
            while (time < fadeDuration)
            {
                Color nextColor = fadeColor;
                nextColor.a = Mathf.Lerp(alphaIn, alphaOut, time / fadeDuration);

                rend.material.SetColor("_Color", nextColor);
                time += Time.deltaTime;
                yield return null;
            }

            Color finalColor = fadeColor;
            finalColor.a = alphaOut;
            rend.material.SetColor("_Color", finalColor);
        }
    }
}
