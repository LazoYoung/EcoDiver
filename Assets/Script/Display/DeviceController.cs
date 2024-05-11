using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Display
{
    public class DeviceController : MonoBehaviour
    {
        [SerializeField] private TMP_Text barometerText;
        [SerializeField] private TMP_Text objectiveText;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private Slider depthSlider;
        [SerializeField] private float maxDepth = 50f;
        private DisplayManager _displayManager;

        private void Start()
        {
            if (barometerText == null || objectiveText == null || progressText == null || depthSlider == null)
            {
                Debug.LogWarning("Device text not assigned!");
                enabled = false;
                return;
            }

            depthSlider.minValue = 0f;
            depthSlider.maxValue = maxDepth;
            _displayManager = DisplayManager.Instance;
        }

        private void Update()
        {
            int oxygen = Mathf.RoundToInt(_displayManager.OxygenRate);
            barometerText.text = $"{Math.Clamp(oxygen, 0, 100)}%";
            objectiveText.text = _displayManager.QuestDescription ?? "N/A";
            progressText.text = _displayManager.QuestLevel.ToString();
            depthSlider.value = maxDepth - _displayManager.WaterDepth;
        }
    }
}
