using UnityEngine;

namespace Script.Quest
{
    public class Quest3A : Quest
    {
        private bool _find;

        protected override void Start()
        {
            base.Start();
            SetupBoxCollider();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_find && other.CompareTag("Player"))
            {
                _find = true;
            }
        }

        protected override bool CanComplete()
        {
            return _find;
        }

        public override string GetQuestName()
        {
            return "Quest 3";
        }

        public override string GetQuestDescription()
        {
            return "Find the robot.";
        }
    }
}
