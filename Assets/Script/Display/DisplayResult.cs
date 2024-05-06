using Script.Quest;

namespace Script.Display
{
    public record DisplayResult
    {
        public DisplayResult(float waterDepth, float oxygenRate, QuestLevel questLevel, string questName, string questDescription)
        {
            WaterDepth = waterDepth;
            OxygenRate = oxygenRate;
            QuestLevel = questLevel;
            QuestName = questName;
            QuestDescription = questDescription;
        }

        public DisplayResult()
        {
        }

        /*
         * Wtaer Depth From SampleHeightHelper
         * If water depth is -80m, result is just 80.0
         *
         * Validation
         * 0.0 ~ Float.MAX
         */
        public float WaterDepth;
        /*
         * Oxygen rate is the rate of oxygen in the water.
         *
         * Validation
         * 0.0 ~ 100.0
         *
         */
        public float OxygenRate;
        /*
         * QuestLevel {CurrentLevel, MaxLevel}
         *
         */
        public QuestLevel QuestLevel = QuestLevel.Empty;
        public string QuestName;
        public string QuestDescription;

        public string toText()
        {
            return $"Water depth: {WaterDepth:F2} | " +
                   $"Oxygen rate: {OxygenRate:F1} | " +
                   $"Quest level: {QuestLevel} | " +
                   $"Quest name: " + QuestName + " | " +
                   $"Quest description: " + QuestDescription + "\n";
        }
    }
}
