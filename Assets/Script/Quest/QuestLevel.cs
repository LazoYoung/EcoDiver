namespace Script.Quest
{
    public class QuestLevel
    {
        private readonly int _maxLevel;
        private int _currentLevel;

        // Getter for _maxLevel
        public int MaxLevel
        {
            get { return _maxLevel; }
        }

        // Getter and setter for _currentLevel
        public int CurrentLevel
        {
            get { return _currentLevel; }
            private set { _currentLevel = value; }
        }

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
