using Script.Quest.Entity;

namespace Script.Quest
{
    public class Quest3A : Quest
    {
        public Robot robot;
        private bool _find;

        protected override void Start()
        {
            base.Start();
            robot.OnChainTied += OnRobotTied;
        }

        private void OnRobotTied(ChainedRobot obj)
        {
            _find = true;
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
