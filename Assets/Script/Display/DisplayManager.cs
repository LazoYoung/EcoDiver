using System;
using Script.Quest;
using UnityEngine;

namespace Script.Display
{
    public class DisplayManager : MonoBehaviour
    {
        // Singleton instance
        private static DisplayManager _instance;
        public static DisplayManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Find existing instance in the scene
                    _instance = FindObjectOfType<DisplayManager>();

                    // Create a new instance if none exists
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("DisplayManager");
                        _instance = obj.AddComponent<DisplayManager>();
                    }
                }
                return _instance;
            }
        }
        // Private fields
        private float waterDepth = 0.0f;
        private float oxygenRate = 100.0f;
        private QuestLevel questLevel = QuestLevel.Empty;

        private string _text;

        public void Update()
        {
            //Test) If press M key, show the display
            if (Input.GetKeyDown(KeyCode.M))
            {
                Debug.Log("Displaying data:" + _text);
            }
        }

        public float WaterDepth
        {
            set
            {
                waterDepth = value;
                UpdateText();
            }
        }

        public float OxygenRate
        {
            set
            {
                oxygenRate = value;
                UpdateText();
            }
        }

        public QuestLevel QuestLevel
        {
            set
            {
                questLevel = value;
                UpdateText();
            }
        }

        // Method to update the text UI components with the current data

        private void UpdateText()
        {
            _text = $"Water depth: {waterDepth:F2} | " +
             $"Oxygen rate: {oxygenRate:F1} | " +
             $"Quest level: {questLevel}\n";
        }
    }
}
