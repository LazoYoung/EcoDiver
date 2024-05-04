using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Image = UnityEngine.UI.Image;
using InputDevice = UnityEngine.XR.InputDevice;

namespace Script.Interaction
{
    public class LocomotionDummy : LocomotionProvider
    {
    }

    public enum Tool
    {
        Hand,
        Knife,
        Headlight,
        Rope
    }

    public class ToolSelector : MonoBehaviour
    {
        [SerializeField] private LocomotionSystem locomotion;

        [SerializeField] private InputActionReference toggleButton;

        [SerializeField] private InputActionReference thumbstickAxis;
        
        [SerializeField] private Canvas canvas;

        [SerializeField] private Image imageHand;

        [SerializeField] private Image imageHeadlight;

        [SerializeField] private Image imageKnife;

        [SerializeField] private Image imageRope;

        [SerializeField] private float idleTime = 5;

        private InputDevice _inputDevice;
        private LocomotionDummy _locomotionDummy;
        private float _timer;
        private bool _isOpen;
        private Tool? _pointer;

        private void Start()
        {
            _locomotionDummy = gameObject.AddComponent<LocomotionDummy>();
            _locomotionDummy.hideFlags = HideFlags.HideAndDontSave;
            
            if (locomotion == null)
                locomotion = FindObjectOfType<LocomotionSystem>();
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
            var temp = GetTemporalSelect();

            if (temp != null)
            {
                _pointer = temp;
            }
            else if (_pointer != null)
            {
                Equip(_pointer.Value);
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
                imageHand.enabled = false;
                imageHeadlight.enabled = false;
                imageKnife.enabled = false;
                imageRope.enabled = false;

                switch (_pointer)
                {
                    case Tool.Hand:
                        imageHand.enabled = true;
                        break;
                    case Tool.Headlight:
                        imageHeadlight.enabled = true;
                        break;
                    case Tool.Knife:
                        imageKnife.enabled = true;
                        break;
                    case Tool.Rope:
                        imageRope.enabled = true;
                        break;
                }
            }
        }

        private Tool? GetTemporalSelect()
        {
            var vector = GetThumbstickAxis();
            var angle = Vector2.SignedAngle(vector, Vector2.up);
            Tool? tool;

            if (vector.sqrMagnitude < 0.5)
            {
                tool = null;
            }
            else if (angle is > -45 and <= 45)
            {
                tool = Tool.Hand;
            }
            else if (angle is > 45 and <= 135)
            {
                tool = Tool.Rope;
            }
            else if (angle is > 135 or < -135)
            {
                tool = Tool.Headlight;
            }
            else
            {
                tool = Tool.Knife;
            }

            return tool;
        }

        private void Equip(Tool tool)
        {
            Debug.Log("Equip: " + tool.ToString());

            // todo method stub
        }

        private void CloseMenu()
        {
            _pointer = null;
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
}
