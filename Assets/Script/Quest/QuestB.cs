using Script.Environment.Collect;
using UnityEngine;

namespace Script.Quest
{
    public class QuestB : Quest
    {
        [SerializeField] private int requiredItems;
        private int RequiredItems => requiredItems;
        private bool _isCompleted;
        
        public override string GetQuestName()
        {
            return "Quest B";
        }

        public override string GetQuestDescription()
        {
            return "Collect the trash from site B.";
        }

        public override bool CanComplete()
        {
            return CollectManager.Instance.GetTotalCollectedItems() >= RequiredItems;
        }
        
        private void Update()
        {
            if (!_isCompleted && CanComplete())
            {
                _isCompleted = true;
                OnComplete();
                Notify();
            }
        }
    }
}
