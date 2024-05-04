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


        public void Update()
        {
            //Test) If press M key, show the display
            if (Input.GetKeyDown(KeyCode.M))
            {
                Debug.Log("Displaying data:" +  $"Water depth: {WaterDepth:F2} | " +
                          $"Oxygen rate: {OxygenRate:F1} | " +
                          $"Quest level: {QuestLevel}\n");
            }
        }

        public float WaterDepth;
        public float OxygenRate;
        public QuestLevel QuestLevel = QuestLevel.Empty;
    }
}
