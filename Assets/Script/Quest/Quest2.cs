using Script.Equipment;
using Script.Interaction;
using UnityEngine;

namespace Script.Quest
{
    public class Quest2 : Quest
    {
        [SerializeField] private Cloth fishingNet;
        [SerializeField] private Equipment.Equipment equipment;
        [SerializeField] private Tool knife;

        private Sliceable _sliceable;
        private bool _teared;

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
            
            if (fishingNet == null)
            {
                Debug.LogWarning("Fishing net is not assigned. Quest2 will not work!");
            }

            if (!fishingNet.TryGetComponent(out _sliceable))
            {
                Debug.LogWarning("Fishing net is not sliceable. Quest2 will not work!");
            }
            else
            {
                _sliceable.onSlice += OnSlice;
            }
        }

        private void OnSlice(Slicer slicer)
        {
            var array = fishingNet.coefficients;
            
            for (var i = 0; i < array.Length; ++i)
            {
                array[i].maxDistance = float.MaxValue;
            }

            fishingNet.coefficients = array;
            fishingNet.enabled = false;
            fishingNet.enabled = true;
            _teared = true;
        }

        protected override bool CanComplete()
        {
            return _teared;
        }

        public override string GetQuestName()
        {
            return "Quest 2";
        }

        public override string GetQuestDescription()
        {
            return "Find the missing turtle to rescue.";
        }
    }
}
