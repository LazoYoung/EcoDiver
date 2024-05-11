using Script.Environment.Collect;

namespace Script.Quest
{
    public class QuestA : Quest
    {
        private readonly int _requiredItemsInGroupA = 2;
        private bool _isCompleted;

        public override string GetQuestName()
        {
            return "Quest A";
        }

        public override string GetQuestDescription()
        {
            return "Press Y to complete Quest A";
        }

        public override bool CanComplete()
        {
            return CollectManager.Instance.GetTotalCollectedItems() >= _requiredItemsInGroupA;
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