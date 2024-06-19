using System;
using System.Collections;
using System.Collections.Generic;
using Scene;
using Script.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Script.Scene
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] [Tooltip("temporary button for loading scene")]
        private InputActionReference loadButton;

        // [SerializeField] [Tooltip("Fade Screen")]
        // private FadeScreen fadeScreen;

        [SerializeField] [Tooltip("Return to Title Scene")]
        private bool returnToTitle = true;

        [SerializeField] [Tooltip("Verbose Mode")]
        private bool verbose;

        [SerializeField] [Tooltip("Verbose Mod")]
        private float secondsToWaitLoad = 1f;

        private List<SceneDetail> _selectedScenario;
        
        private bool _isLoadingScene;

        private FadeScreen _fadeScreen;
        
        private static SceneLoader _instance;

        public static SceneLoader Instance
        {
            get
            {
                if (!_instance)
                {
                    SetupInstance();
                }

                return _instance;
            }
        }

        private static void SetupInstance()
        {
            _instance = FindObjectOfType<SceneLoader>();
            
            if (!_instance)
            {
                GameObject gameObj = new GameObject();
                gameObj.name = "SceneLoader";
                _instance = gameObj.AddComponent<SceneLoader>();
                DontDestroyOnLoad(gameObj);
            }
        }

        private readonly List<SceneDetail> ProductionScenes = new List<SceneDetail>
        {
            //Example
            SceneDetail.TitleScene, SceneDetail.PrologueScene, SceneDetail.Quest1Scene,
            SceneDetail.Quest2Scene, SceneDetail.Quest3Scene //, SceneDetail.TitleScene
        };

        private readonly List<SceneDetail> TestScenes = new List<SceneDetail>
        {
            SceneDetail.TestStartScene, SceneDetail.TestMainScene, SceneDetail.TestEndScene
        };
        private Coroutine _holdButtonCoroutine;


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

        private FadeScreen GetFadeScreen()
        {
            if (!_fadeScreen)
            {
                _fadeScreen = FindObjectOfType<FadeScreen>(true);
            }

            return _fadeScreen;
        }

        private void LoadInstance()
        {
            if (verbose)
            {
                Debug.Log("Awake Singleton");
            }

            if (_instance == null)
            {
                if (verbose)
                {
                    Debug.Log("Instance is null");
                }

                _instance = this;
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
                _selectedScenario = TestScenes;
                if (verbose)
                {
                    Debug.Log("Selected Scene is Test Scene");
                }
            }
            else if (DoExistInScenario(ProductionScenes, SceneManager.GetActiveScene()))
            {
                _selectedScenario = ProductionScenes;
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

            GetFadeScreen().gameObject.SetActive(true);
        }

        public void LoadNextScene()
        {
            int currentSceneIndex = FindSceneIndex(_selectedScenario, SceneManager.GetActiveScene());
            if (currentSceneIndex == -1)
            {
                Debug.LogError("Current scene is not exist in scenario");
                return;
            }

            if (_selectedScenario.Count <= currentSceneIndex + 1)
            {
                if (!returnToTitle)
                {
                    Debug.LogError("Next scene is not exist.");
                    return;
                }

                LoadNewScene(_selectedScenario[0].Name);
                return;
            }

            LoadNewScene(_selectedScenario[currentSceneIndex + 1].Name);
        }

        private void LoadNewScene(string sceneName)
        {
            if (_isLoadingScene)
            {
                Debug.Log("Loading Scene Failed By Loading : " + sceneName);
                return;
            }

            Debug.Log("Loading Scene: " + sceneName);
            StartCoroutine(LoadSceneWithDelay(sceneName));
        }

        private void Update()
        {
            if (loadButton && loadButton.action.WasPressedThisFrame())
            {
                OnLoadButtonPressed();
            }
        }

        private void OnLoadButtonPressed()
        {
            if (_holdButtonCoroutine != null)
                return;

            _holdButtonCoroutine = StartCoroutine(LoadButtonCoroutine());
        }
        private IEnumerator LoadButtonCoroutine()
        {
            yield return new WaitForSeconds(1f);

            _holdButtonCoroutine = null;
            
            if (loadButton.action.IsPressed())
            {
                LoadNextScene();
            }
        }

        private IEnumerator LoadSceneWithDelay(string sceneName)
        {
            _isLoadingScene = true;
            yield return new WaitForSeconds(secondsToWaitLoad); // Wait for 1 second before starting to load the scene
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            if (verbose)
            {
                Debug.Log("To Load Scene's name : " + sceneName);
            }

            FadeScreen fadeScreen = GetFadeScreen();
            fadeScreen.FadeOut();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            float timer = 0;
            while ((fadeScreen && timer <= fadeScreen.FadeDuration) && !asyncLoad.isDone)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            asyncLoad.allowSceneActivation = true;
            asyncLoad.completed += OnSceneLoaded;
            _isLoadingScene = false;
        }

        private void OnSceneLoaded(AsyncOperation obj)
        {
            GetFadeScreen().gameObject.SetActive(true);
            SettingsManager.Instance.Reload();
        }
    }
}
