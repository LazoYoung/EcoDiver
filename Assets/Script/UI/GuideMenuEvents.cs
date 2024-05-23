using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Script.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class GuideMenuEvents : MonoBehaviour
    {
        [SerializeField] private GameObject mission1_UI;
        [SerializeField] private int transitionSeconds = 7;
        private VisualElement[] _guidePanels;
        private VisualElement _mainPanel;
        private VisualElement _aboutPanel;
        private VisualElement _root;
        private int _page;

        private void Start()
        {
            if (mission1_UI == null)
            {
                Debug.LogWarning("Mission 1 is not assigned!");
            }
            
            var document = GetComponent<UIDocument>();
            _root = document.rootVisualElement;
            _mainPanel = _root.Q<VisualElement>("Main");
            _aboutPanel = _root.Q<VisualElement>("About");
            _guidePanels = new[]
            {
                _root.Q<VisualElement>("Play0"),
                _root.Q<VisualElement>("Play1"),
                _root.Q<VisualElement>("Play2"),
                _root.Q<VisualElement>("Play3"),
                _root.Q<VisualElement>("Play4")
            };
            var guideButton = _root.Q<Button>("GuideButton");
            var aboutButton = _root.Q<Button>("AboutButton");
            var closeButton = _root.Query<Button>("CloseButton");
            var leftButton = _root.Query<Button>("LeftButton");
            var rightButton = _root.Query<Button>("RightButton");
            
            guideButton.RegisterCallback<ClickEvent>(OnClickGuide);
            aboutButton.RegisterCallback<ClickEvent>(OnClickAbout);
            closeButton.ForEach(button => button.RegisterCallback<ClickEvent>(OnClickClose));
            leftButton.ForEach(button => button.RegisterCallback<ClickEvent>(OnClickLeft));
            rightButton.ForEach(button => button.RegisterCallback<ClickEvent>(OnClickRight));

            if (mission1_UI != null)
            {
                gameObject.SetActive(true);
            }
            
            ShowPanel(_mainPanel);
        }

        private void HidePanels()
        {
            _root.Query<VisualElement>(className: "panel").ForEach(element =>
            {
                element.style.display = DisplayStyle.None;
                Debug.Log($"{element.name} invisible");
            });
        }

        private void ShowPanel(VisualElement panel)
        {
            HidePanels();
            panel.style.display = DisplayStyle.Flex;
            Debug.Log($"{panel.name} visible");
        }
        
        private void OnClickRight(ClickEvent evt)
        {
            _page = Math.Clamp(_page + 1, 0, _guidePanels.Length - 1);
            ShowPanel(_guidePanels[_page]);

            if (_page == _guidePanels.Length - 1 && mission1_UI != null)
            {
                StartCoroutine(TransitionToMission());
            }
        }

        private IEnumerator TransitionToMission()
        {
            yield return new WaitForSeconds(transitionSeconds);
            
            mission1_UI.SetActive(true);
            gameObject.SetActive(false);
        }

        private void OnClickLeft(ClickEvent evt)
        {
            _page = Math.Clamp(_page - 1, 0, _guidePanels.Length - 1);
            ShowPanel(_guidePanels[_page]);
        }

        private void OnClickClose(ClickEvent evt)
        {
            ShowPanel(_mainPanel);
        }

        private void OnClickAbout(ClickEvent evt)
        {
            ShowPanel(_aboutPanel);
        }

        private void OnClickGuide(ClickEvent evt)
        {
            _page = 0;
            ShowPanel(_guidePanels[0]);
        }
    }
}