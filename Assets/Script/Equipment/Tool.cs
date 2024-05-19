using System;
using UnityEngine;

namespace Script.Equipment
{
    [CreateAssetMenu(fileName = "Tool", menuName = "Scriptable Object/Tool", order = 0)]
    public class Tool : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private GameObject forearmPrefab;

        public string Identifier => id;
        public GameObject ForearmPrefab => forearmPrefab;
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tool) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Identifier);
        }

        private bool Equals(Tool other)
        {
            return base.Equals(other) && Identifier == other.Identifier;
        }
    }
}
