using Script.Quest.Entity;
using UnityEngine;

namespace Script.Quest
{
    public class Quest3B : Quest
    {
        public Robot robot;
        private Robot _robot;
        private ChainedRobot _chainedRobot;
        private bool _inside;
        private bool _canComplete;

        public override string GetQuestName()
        {
            return "Quest 3";
        }

        public override string GetQuestDescription()
        {
            return "Retrieve the robot.";
        }

        protected override bool CanComplete()
        {
            return _canComplete;
        }

        protected override void Start()
        {
            base.Start();
            SetupBoxCollider();
            SetupRobot(robot);
        }

        private void SetupRobot(Robot instance)
        {
            _robot = instance;
            _robot.OnChainTied += OnRobotTied;
        }

        private void OnRobotTied(ChainedRobot chainedRobot)
        {
            _chainedRobot = chainedRobot;
            _chainedRobot.OnChainUntied += OnRobotUntied;
        }

        private void OnRobotUntied(Robot instance)
        {
            SetupRobot(instance);
            _chainedRobot.OnChainUntied -= OnRobotUntied;
            _chainedRobot = null;

            if (_inside)
            {
                _canComplete = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _inside = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _inside = false;
            }
        }
    }
}
