namespace Script.Quest
{
    public class QuestLevel
    {
        private readonly int _maxLevel;
        private int _currentLevel;

        public static QuestLevel Empty = new QuestLevel(0);

        public QuestLevel(int maxLevel)
        {
            _maxLevel = maxLevel;
            _currentLevel = 0;
        }

        public void LevelUp()
        {
            if (_currentLevel < _maxLevel)
            {
                _currentLevel++;
            }
        }

        public override string ToString()
        {
            return _currentLevel + "/" + _maxLevel;
        }
    }
}
