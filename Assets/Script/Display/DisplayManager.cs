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
                Debug.Log("Displaying data:" + DisplayManager.Instance.GetDisplayResult());
            }
        }

        private DisplayResult _displayResult = new DisplayResult();

        public float WaterDepth { get => _displayResult.WaterDepth; set => _displayResult.WaterDepth = value; }
        public float OxygenRate { get => _displayResult.OxygenRate; set => _displayResult.OxygenRate = value;}
        public QuestLevel QuestLevel { get => _displayResult.QuestLevel; set => _displayResult.QuestLevel = value;}
        public string QuestName { get => _displayResult.QuestName; set => _displayResult.QuestName = value;}
        public string QuestDescription { get => _displayResult.QuestDescription; set => _displayResult.QuestDescription = value;}

        public DisplayResult GetDisplayResult()
        {
            return _displayResult;
        }
    }
}
