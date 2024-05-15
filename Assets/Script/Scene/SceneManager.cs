using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class SceneLoader : MonoBehaviour
    {
        private List<SceneDetail> selectedScenario;

        [SerializeField] [Tooltip("temporary button for loading scene")]
        private InputActionReference loadButton;

        [SerializeField] [Tooltip("Fade Screen")]
        private FadeScreen fadeScreen;

        private readonly List<SceneDetail> ProductionScenes = new List<SceneDetail>
        {
            //Example
            SceneDetail.StartScene, SceneDetail.MainScene, SceneDetail.EndScene
        };

        private readonly List<SceneDetail> TestScenes = new List<SceneDetail>
        {
            SceneDetail.TestStartScene, SceneDetail.TestMainScene, SceneDetail.TestEndScene
        };

        private static int FindSceneIndex(List<SceneDetail> sceneDetails,
            UnityEngine.SceneManagement.Scene currentScene)
        {
            return sceneDetails.FindIndex(scene => currentScene.path.Contains(scene.Name));
        }

        private static bool IsExistInScenario(List<SceneDetail> sceneDetails,
            UnityEngine.SceneManagement.Scene currentScene)
        {
            return FindSceneIndex(sceneDetails, currentScene) != -1;
        }

        void Awake()
        {
            if (IsExistInScenario(TestScenes, SceneManager.GetActiveScene()))
            {
                selectedScenario = TestScenes;
                Debug.Log("Selected Scene is Test Scene");
            }
            else if (IsExistInScenario(ProductionScenes, SceneManager.GetActiveScene()))
            {
                selectedScenario = ProductionScenes;
                Debug.Log("Selected Scene is Production Scene");
            }
            else
            {
                Debug.LogWarning("Selected Scene is not exist in scenario");
                enabled = false;
            }
            if (fadeScreen == null)
            {
                Debug.LogWarning("Fade Screen is not set.");
            }
            else
            {
                fadeScreen.gameObject.SetActive(true);
            }
        }

        public void LoadNextScene()
        {
            int currentSceneIndex = FindSceneIndex(selectedScenario, SceneManager.GetActiveScene());
            if (currentSceneIndex == -1)
            {
                Debug.LogError("Current scene is not exist in scenario");
                return;
            }
            if (selectedScenario.Count <= currentSceneIndex + 1)
            {
                Debug.LogError("Next scene is not exist.");
                return;
            }

            LoadNewScene(selectedScenario[currentSceneIndex + 1].Name);
        }

        private void LoadNewScene(string sceneName)
        {
            Debug.Log("Loading Scene: " + sceneName);
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        // if press button h to load scene
        private void Update()
        {
            //테스트 용도 실 사용 계획 X
            if (loadButton && loadButton.action.WasPressedThisFrame())
            {
                Debug.Log("Loading Scene Triggered by Button Pressed");
                LoadNextScene();
            }
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            Debug.Log("To Load Scene's name : " + sceneName);
            if (fadeScreen == null)
            {
                Debug.LogWarning("Fade Screen is not set.");
            } else
            {
                fadeScreen.FadeOut();
            }
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            float timer = 0;
            while ((fadeScreen != null && timer <= fadeScreen.FadeDuration) && !asyncLoad.isDone)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            asyncLoad.allowSceneActivation = true;
        }
    }
}
