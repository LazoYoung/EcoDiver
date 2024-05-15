using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class SceneLoader : MonoBehaviour
    {
        private List<SceneDeatil> selectedScenario;

        private string loadingText = "Loading...";
        [SerializeField] [Tooltip("Fade Screen Object")]
        private FadeScreen fadeScreen;

        private readonly List<SceneDeatil> ProductionScenes = new List<SceneDeatil>
        {
            SceneDeatil.StartScene, SceneDeatil.MainScene, SceneDeatil.EndScene
        };

        private readonly List<SceneDeatil> TestScenes = new List<SceneDeatil>
        {
            SceneDeatil.TestStartScene, SceneDeatil.TestMainScene, SceneDeatil.TestEndScene
        };

        private static int FindSceneIndex(List<SceneDeatil> sceneDeatils,
            UnityEngine.SceneManagement.Scene currentScene)
        {
            return sceneDeatils.FindIndex(scene => currentScene.path.Contains(scene.Name));
        }

        private static bool IsExistInScenario(List<SceneDeatil> sceneDeatils,
            UnityEngine.SceneManagement.Scene currentScene)
        {
            return FindSceneIndex(sceneDeatils, currentScene) != -1;
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
                Debug.LogError("Selected Scene is not exist in scenario");
                enabled = false;
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
            if (Input.GetKeyDown(KeyCode.H))
            {
                //메인씬 로딩 시간 4초
                Debug.Log("Loading Scene H ");
                LoadNewScene("Level/Production/MainScene");
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log("Loading Scene J");
                LoadNextScene();
            }
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            Debug.Log("Load Scene Async: " + sceneName);
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