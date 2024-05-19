using Script.Equipment;
using Script.Interaction;
using UnityEngine;

namespace Script.Quest
{
    public class Quest2 : Quest
    {
        [SerializeField] private Tool knife;
        [SerializeField] private Equipment.Equipment equipment;
        [SerializeField] private Turtle turtle;

        private Sliceable _fishingNet;
        private bool _teared;
        
        public override string GetQuestName()
        {
            return "Quest 2";
        }

        public override string GetQuestDescription()
        {
            return "Find the missing turtle to rescue.";
        }
        
        protected override bool CanComplete()
        {
            return _teared;
        }
        
        protected override void Start()
        {
            base.Start();

            if (equipment == null)
            {
                equipment = FindObjectOfType<Equipment.Equipment>();
            }

            if (knife == null)
            {
                Debug.LogWarning("Knife tool is not assigned. Quest2 will not work!");
            }
            
            if (turtle == null)
            {
                turtle = FindObjectOfType<Turtle>();
            }

            if (turtle == null)
            {
                Debug.LogWarning("Turtle is not assigned. Quest2 will not work!");
            }
            else
            {
                _fishingNet = turtle.fishingNet;
                _fishingNet.OnSlice += OnSlice;
            }
        }
        
        private void OnSlice(Slicer slice)
        {
            var cloth = _fishingNet.GetComponent<Cloth>();
            var array = cloth.coefficients;
            
            for (var i = 0; i < array.Length; ++i)
            {
                array[i].maxDistance = float.MaxValue;
            }

            cloth.coefficients = array;
            cloth.enabled = false;
            cloth.enabled = true;
            _teared = true;
        }
    }
}
