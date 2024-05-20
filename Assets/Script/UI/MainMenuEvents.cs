using System.Collections;
using System.Collections.Generic;
using Script.Scene;
using UnityEngine;
using UnityEngine.UIElements;

namespace Script.UI
{
    public class MainMenuEvents : MonoBehaviour
    {
        [SerializeField] [Tooltip("Verbose Mode")]
        private bool verbose = true;

        private UIDocument _uiDocument;

        private Button _startButton;
        private Button _settingsButton;
        private Button _creditsButton;
        private Button _exitButton;

        [SerializeField]
        private GameObject settingsUI; // Reference to the Settings UI GameObject

        [SerializeField]
        private GameObject creditsUI; // Reference to the Credits UI GameObject

        // Start is called before the first frame update
        void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            Debug.Log("UI Document: " + _uiDocument);
            _startButton = _uiDocument.rootVisualElement.Q<Button>("StartButton");
            if (_startButton == null)
            {
                Debug.LogError("Start Button is null!");
            }

            _startButton.RegisterCallback<ClickEvent>(onStartButtonClicked);

            _settingsButton = _uiDocument.rootVisualElement.Q<Button>("SettingsButton");
            if (_settingsButton == null)
            {
                Debug.LogError("Settings Button is null!");
            }

            _settingsButton.RegisterCallback<ClickEvent>(onSettingsButtonClicked);

            _creditsButton = _uiDocument.rootVisualElement.Q<Button>("CreditsButton");
            if (_creditsButton == null)
            {
                Debug.LogError("Credits Button is null!");
            }

            _creditsButton.RegisterCallback<ClickEvent>(onCreditsButtonClicked);

            _exitButton = _uiDocument.rootVisualElement.Q<Button>("ExitButton");
            if (_exitButton == null)
            {
                Debug.LogError("Exit Button is null!");
            }

            _exitButton.RegisterCallback<ClickEvent>(onExitButtonClicked);

            // Ensure Settings and Credits UI are initially hidden
            if (settingsUI != null) settingsUI.SetActive(false);
            if (creditsUI != null) creditsUI.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void onStartButtonClicked(ClickEvent clickEvent)
        {
            if (verbose)
            {
                Debug.Log("Start Button Clicked!");
            }

            SceneLoader.Instance.LoadNextScene();
        }

        private void onSettingsButtonClicked(ClickEvent clickEvent)
        {
            if (verbose) Debug.Log("Settings Button Clicked!");
            ShowSettings();
        }

        private void onCreditsButtonClicked(ClickEvent clickEvent)
        {
            if (verbose) Debug.Log("Credits Button Clicked!");
            ShowCredits();
        }

        private void onExitButtonClicked(ClickEvent clickEvent)
        {
            if (verbose) Debug.Log("Exit Button Clicked!");
            Application.Quit();
        }

        private void ShowSettings()
        {
            if (settingsUI != null)
            {
                settingsUI.SetActive(true);
            }

            if (creditsUI != null)
            {
                creditsUI.SetActive(false);
            }
        }

        private void ShowCredits()
        {
            if (creditsUI != null)
            {
                creditsUI.SetActive(true);
            }

            if (settingsUI != null)
            {
                settingsUI.SetActive(false);
            }
        }
    }
}
