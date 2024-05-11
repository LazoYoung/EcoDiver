using Script.Environment.Collect;

namespace Script.Quest
{
    public class QuestC : Quest
    {
        private readonly int _requiredItemsInGroupC = 5;
        private bool _isCompleted;
        
        public override string GetQuestName()
        {
            return "Quest C";
        }

        public override string GetQuestDescription()
        {
            return "Press I to complete Quest C";
        }
        
        public override bool CanComplete()
        {
            return CollectManager.Instance.GetTotalCollectedItems() >= _requiredItemsInGroupC;
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
