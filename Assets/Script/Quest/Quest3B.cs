using UnityEngine;

namespace Script.Quest
{
    public class Quest3B : Quest
    {
        private bool _retrieve;

        protected override void Start()
        {
            base.Start();
            SetupBoxCollider();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_retrieve && other.CompareTag("Player"))
            {
                _retrieve = true;
            }
        }
        
        protected override bool CanComplete()
        {
            // todo: check if player has put the robot on the floor
            return _retrieve;
        }

        public override string GetQuestName()
        {
            return "Quest 3";
        }

        public override string GetQuestDescription()
        {
            return "Retrieve the robot.";
        }
    }
}
