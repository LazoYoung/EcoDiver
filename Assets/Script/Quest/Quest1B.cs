using Script.Environment.Collect;
using UnityEngine;

namespace Script.Quest
{
    public class Quest1B : Quest
    {
        [SerializeField] private int requiredItems;
        private int RequiredItems => requiredItems;
        
        public override string GetQuestName()
        {
            return "Quest 1B";
        }

        public override string GetQuestDescription()
        {
            return "Collect the trash from site B.";
        }

        protected override bool CanComplete()
        {
            return CollectManager.Instance.GetTotalCollectedItems() >= RequiredItems;
        }
        
        protected override void OnComplete()
        {
            base.OnComplete();
            CollectManager.Instance.ResetCount();
        }
    }
}
