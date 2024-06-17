using System.Collections;
using System.Collections.Generic;
using Script.UI;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static Unity.XR.CoreUtils.XROrigin.TrackingOriginMode;
using Debug = UnityEngine.Debug;

namespace Script.Interaction
{
    /// <summary>
    ///     Workaround for this issue https://discussions.unity.com/t/xr-rig-tracking-origin-mode-floor-doesnt-align-after-restart/336353
    ///     This issue seemed to happen only when putting the headset after the Editor started to play
    /// </summary>
    public class XROriginFloorFix : MonoBehaviour
    {
        [SerializeField] private XROrigin xrOrigin;

        [SerializeField] private InputActionReference switchMode;

        private Coroutine _resetCoroutine;
        
        private InputAction _switchButton;
        
        private void Start()
        {
            if (switchMode == null)
            {
                Debug.LogError("Switch button is not assigned!");
            }
            else
            {
                _switchButton = switchMode.action;
            }
        }

        private void Update()
        {
            if (!SettingsManager.Instance.IsFloorInitialized && IsHardwarePresent())
            {
                StartCoroutine(InitializeCoroutine());
                SettingsManager.Instance.IsFloorInitialized = true;
            }

            if (_resetCoroutine == null && _switchButton.WasPressedThisFrame())
            {
                _resetCoroutine = StartCoroutine(SwitchCoroutine());
            }
        }

        private IEnumerator SwitchCoroutine()
        {
            yield return new WaitForSeconds(1f);

            if (_switchButton.IsPressed())
            {
                switch (xrOrigin.RequestedTrackingOriginMode)
                {
                    case Device:
                        SettingsManager.Instance.TrackingMode = Floor;
                        break;
                    case Floor:
                        SettingsManager.Instance.TrackingMode = Device;
                        break;
                }

                Debug.Log($"New tracking mode: {SettingsManager.Instance.TrackingMode}");
            }

            _resetCoroutine = null;
        }
        
        private IEnumerator InitializeCoroutine()
        {
            SettingsManager.Instance.TrackingMode = Device;
            yield return new WaitForSeconds(0.1f);
            SettingsManager.Instance.TrackingMode = Floor;
        }

        public static bool IsHardwarePresent()
        {
            var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
            SubsystemManager.GetInstances(xrDisplaySubsystems);

            foreach (var xrDisplay in xrDisplaySubsystems)
            {
                if (xrDisplay.running)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
