using System.Collections.Generic;

namespace Scene
{
    public struct SceneDeatil
    {
        public readonly string Name;
        public readonly string Description;

        public static SceneDeatil StartScene =
            new SceneDeatil("Level/Production/StartScene", "This is the starting scene of the game.");
        public static SceneDeatil MainScene =
            new SceneDeatil("Level/Production/MainScene", "This is the starting scene of the game.");
        public static SceneDeatil EndScene =
            new SceneDeatil("Level/Production/EndScene", "This is the starting scene of the game.");

        public static SceneDeatil TestStartScene =
            new SceneDeatil("Level/Experiment/Test/StartScene", "This is the starting scene of the test game.");
        public static SceneDeatil TestMainScene =
            new SceneDeatil("Level/Experiment/Test/MainScene", "This is the starting scene of the test game.");
        public static SceneDeatil TestEndScene =
            new SceneDeatil("Level/Experiment/Test/EndScene", "This is the starting scene of the test game.");

        public SceneDeatil(string name, string description)
        {
            Name = name;
            Description = description;
        }

    }
}
