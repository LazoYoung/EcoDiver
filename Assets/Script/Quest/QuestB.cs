using Script.Environment.Collect;
using UnityEngine;

namespace Script.Quest
{
    public class QuestB : Quest
    {
        private readonly int _requiredItemsInGroupB = 4;
        private bool _isCompleted;
        
        public override string GetQuestName()
        {
            return "Quest B";
        }

        public override string GetQuestDescription()
        {
            return "Press U to complete Quest B";
        }

        public override bool CanComplete()
        {
            return CollectManager.Instance.GetTotalCollectedItems() >= _requiredItemsInGroupB;
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
