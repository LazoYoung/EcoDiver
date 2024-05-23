using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using zScene;

namespace Script.Scene
{
    public class SceneLoader : MonoBehaviour
    {
        private List<SceneDetail> selectedScenario;

        [SerializeField] [Tooltip("temporary button for loading scene")]
        private InputActionReference loadButton;

        [SerializeField] [Tooltip("Fade Screen")]
        private FadeScreen fadeScreen;

        [SerializeField] [Tooltip("Return to Title Scene")]
        private bool returnToTitle = true;

        [SerializeField] [Tooltip("Verbose Mode")]
        private bool verbose = false;

        private bool isLoadingScene = false;

        private static SceneLoader instance;

        public static SceneLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    SetupInstance();
                }

                return instance;
            }
        }

        private static void SetupInstance()
        {
            instance = FindObjectOfType<SceneLoader>();
            if (instance == null)
            {
                GameObject gameObj = new GameObject();
                gameObj.name = "SceneLoader";
                instance = gameObj.AddComponent<SceneLoader>();
                DontDestroyOnLoad(gameObj);
            }
        }

        private readonly List<SceneDetail> ProductionScenes = new List<SceneDetail>
        {
            //Example
            SceneDetail.TitleScene, SceneDetail.PrologueScene, SceneDetail.Quest1Scene, SceneDetail.Quest2Scene//, SceneDetail.TitleScene
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

        private static bool DoExistInScenario(List<SceneDetail> sceneDetails,
            UnityEngine.SceneManagement.Scene currentScene)
        {
            return FindSceneIndex(sceneDetails, currentScene) != -1;
        }

        void Awake()
        {
            LoadInstance();
            LoadScene();
        }

        private void LoadInstance()
        {
            if (verbose)
            {
                Debug.Log("Awake Singleton");
            }
            if (instance == null)
            {
                if (verbose)
                {
                    Debug.Log("Instance is null");
                }
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                if (verbose)
                {
                    Debug.Log("Instance is not null");
                }

                Destroy(gameObject);
            }
        }

        private void LoadScene()
        {
            if (DoExistInScenario(TestScenes, SceneManager.GetActiveScene()))
            {
                selectedScenario = TestScenes;
                if (verbose)
                {
                    Debug.Log("Selected Scene is Test Scene");
                }
            }
            else if (DoExistInScenario(ProductionScenes, SceneManager.GetActiveScene()))
            {
                selectedScenario = ProductionScenes;
                if (verbose)
                {
                    Debug.Log("Selected Scene is Production Scene");
                }
            }
            else
            {
                if (verbose)
                {
                    Debug.LogWarning("Selected Scene doesn't exist in scenario");
                }
                enabled = false;
            }

            if (fadeScreen == null)
            {
                if (verbose)
                {
                    Debug.LogWarning("Fade Screen is not set.");
                }
            }
            else
            {
                if (verbose)
                {
                    Debug.Log("Fade screen found.");
                }

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
                if (!returnToTitle)
                {
                    Debug.LogError("Next scene is not exist.");
                    return;
                }
                LoadNewScene(selectedScenario[0].Name);
                return;
            }

            LoadNewScene(selectedScenario[currentSceneIndex + 1].Name);
        }

        private void LoadNewScene(string sceneName)
        {
            if (isLoadingScene)
            {
                Debug.Log("Loading Scene Failed By Loading : " + sceneName);
                return;
            }

            Debug.Log("Loading Scene: " + sceneName);
            StartCoroutine(LoadSceneWithDelay(sceneName));
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

        private IEnumerator LoadSceneWithDelay(string sceneName)
        {
            isLoadingScene = true;
            yield return new WaitForSeconds(1f); // Wait for 1 second before starting to load the scene
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            if (verbose)
            {
                Debug.Log("To Load Scene's name : " + sceneName);
            }

            if (fadeScreen == null)
            {
                Debug.LogWarning("Fade Screen is not set.");
            }
            else
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
            isLoadingScene = false;
        }
    }
}
