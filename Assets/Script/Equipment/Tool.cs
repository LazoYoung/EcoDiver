using UnityEngine;

namespace Script.Equipment
{
    [CreateAssetMenu(fileName = "Tool", menuName = "Scriptable Object/Tool", order = 0)]
    public class Tool : ScriptableObject
    {
        [SerializeField] private string toolName;
        [SerializeField] private GameObject forearmPrefab;

        public string Name => toolName;
        public GameObject ForearmPrefab => forearmPrefab;
    }
}
