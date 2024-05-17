using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using InputDevice = UnityEngine.XR.InputDevice;

namespace Script.Equipment
{
    public class ToolSelector : MonoBehaviour
    {
        [SerializeField] private LocomotionSystem locomotion;

        [SerializeField] private InputActionReference toggleButton;

        [SerializeField] private InputActionReference thumbstickAxis;
        
        [SerializeField] private Canvas canvas;

        [SerializeField] private Image[] buttonImages;

        [SerializeField] private Equipment equipment;

        [SerializeField] private Tool[] tools;

        [SerializeField] private float idleTime = 5;

        private InputDevice _inputDevice;
        private LocomotionDummy _locomotionDummy;
        private float _timer;
        private bool _isOpen;
        private Tool _selected;
        private int _selectedIdx;

        private void Start()
        {
            _locomotionDummy = gameObject.AddComponent<LocomotionDummy>();
            _locomotionDummy.hideFlags = HideFlags.HideAndDontSave;
            
            if (locomotion == null)
                locomotion = FindObjectOfType<LocomotionSystem>();
            
            Array.Resize(ref tools, 4);
            Array.Resize(ref buttonImages, 4);
        }

        private void OnDestroy()
        {
            Destroy(_locomotionDummy);
        }

        private void Update()
        {
            if (_isOpen)
            {
                UpdateMenu();
            }
            else if (IsMenuToggled() && DisableLocomotion())
            {
                _isOpen = true;
            }

            UpdateUI();
        }

        private void UpdateMenu()
        {
            _timer += Time.deltaTime;
            int index = GetTemporalSelect();

            if (index > -1)
            {
                _selectedIdx = index;
                _selected = tools[index];
            }
            else if (_selected)
            {
                equipment.Equip(_selected);
                CloseMenu();
                EnableLocomotion();
            }
            else if (IsTimeOver() || IsMenuToggled())
            {
                CloseMenu();
                ResetTimer();
                EnableLocomotion();
            }
        }

        private void UpdateUI()
        {
            canvas.enabled = _isOpen;

            if (_isOpen)
            {
                DeactivateButtons();

                if (_selected)
                {
                    ActivateButton();
                }
            }
        }
        
        private int GetTemporalSelect()
        {
            var vector = GetThumbstickAxis();
            float angle = Vector2.SignedAngle(vector, Vector2.up);

            if (vector.sqrMagnitude < 0.5)
            {
                return -1;
            }
            else if (angle is > -45 and <= 45)
            {
                return 0;
            }
            else if (angle is > 45 and <= 135)
            {
                return 1;
            }
            else if (angle is > 135 or < -135)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        private void CloseMenu()
        {
            _selected = null;
            _selectedIdx = 0;
            _isOpen = false;
        }

        private bool IsMenuToggled()
        {
            return toggleButton.action.WasPressedThisFrame();
        }

        private Vector2 GetThumbstickAxis()
        {
            return thumbstickAxis.action.ReadValue<Vector2>();
        }

        private bool IsTimeOver()
        {
            return _isOpen && _timer > idleTime;
        }

        private void ResetTimer()
        {
            _timer = 0;
        }
        
        private void DeactivateButtons()
        {
            foreach (var buttonImage in buttonImages)
            {
                if (buttonImage)
                {
                    buttonImage.enabled = false;
                }
            }
        }

        private void ActivateButton()
        {
            var buttonImage = buttonImages[_selectedIdx];

            if (buttonImage)
            {
                buttonImage.enabled = true;
            }
        }

        private bool DisableLocomotion()
        {
            var result = locomotion.RequestExclusiveOperation(_locomotionDummy);

            if (result != RequestResult.Success)
            {
                Debug.Log($"Locomotion request failed: {result}");
            }
            return result == RequestResult.Success;
        }

        private void EnableLocomotion()
        {
            locomotion?.FinishExclusiveOperation(_locomotionDummy);
        }
    }
    
    public class LocomotionDummy : LocomotionProvider
    {
    }
}
