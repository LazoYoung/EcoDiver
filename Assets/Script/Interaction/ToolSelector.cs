using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

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
        [SerializeField] private InputActionReference toggleButton;

        [SerializeField] private InputActionReference thumbStick;

        [SerializeField] private LocomotionSystem locomotion;

        [SerializeField] private float idleTime = 5;

        private LocomotionDummy _locomotionDummy;
        private float _timer;
        private bool _isOpen;
        private Tool? _pointer;

        private void Start()
        {
            if (!toggleButton)
            {
                Debug.LogError("Toggle button action not assigned!");
                enabled = false;
            }

            if (!thumbStick)
            {
                Debug.LogError("Thumbstick action not assigned!");
                enabled = false;
            }

            if (!locomotion)
                locomotion = FindObjectOfType<LocomotionSystem>();
        }

        private void Update()
        {
            if (!_isOpen)
            {
                if (IsButtonPressed() && DisableLocomotion())
                {
                    OpenMenu();
                }

                return;
            }

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
            else if (IsTimeOver() || IsButtonPressed())
            {
                CloseMenu();
                ResetTimer();
                EnableLocomotion();
            }
        }

        private Tool? GetTemporalSelect()
        {
            var vector = thumbStick.action.ReadValue<Vector2>();
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
                tool = Tool.Knife;
            }
            else if (angle is > 135 or < -135)
            {
                tool = Tool.Headlight;
            }
            else
            {
                tool = Tool.Rope;
            }

            return tool;
        }

        private void Equip(Tool tool)
        {
            Debug.Log("Equip: " + tool.ToString());

            // todo method stub
        }

        private void OpenMenu()
        {
            _isOpen = true;

            Debug.Log("Menu open.");
            // todo method stub
        }

        private void CloseMenu()
        {
            _pointer = null;
            _isOpen = false;

            Debug.Log("Menu close.");
            // todo method stub
        }

        private bool IsButtonPressed()
        {
            return toggleButton.action.WasPressedThisFrame();
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
            if (!locomotion)
                return true;

            var result = locomotion.RequestExclusiveOperation(_locomotionDummy);
            return result == RequestResult.Success;
        }

        private void EnableLocomotion()
        {
            locomotion?.FinishExclusiveOperation(_locomotionDummy);
        }
    }
}
