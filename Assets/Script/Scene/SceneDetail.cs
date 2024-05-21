using System.Collections.Generic;

namespace Script.Scene
{
    public struct SceneDetail
    {
        public readonly string Name;
        public readonly string Description;

        //Production Scenes
        public static readonly SceneDetail TitleScene =
            new SceneDetail("Level/Production/TitleScene", "This is the title scene of the game.");
        public static readonly SceneDetail PrologueScene =
            new SceneDetail("Level/Production/PrologueScene", "This is the Prologue scene of the game.");
        public static readonly SceneDetail Quest1Scene =
            new SceneDetail("Level/Production/Quest1Scene", "This is the first quest scene of the game.");
        public static readonly SceneDetail Quest2Scene =
            new SceneDetail("Level/Production/Quest2Scene", "This is the second quest scene of the game.");
        public static readonly SceneDetail EndingScene =
            new SceneDetail("Level/Production/EndingScene", "This is the ending scene of the game.");

        // Test Scenes
        public static readonly SceneDetail TestStartScene =
            new SceneDetail("Level/Experiment/Test/StartScene", "This is the starting scene of the test game.");
        public static readonly SceneDetail TestMainScene =
            new SceneDetail("Level/Experiment/Test/MainScene", "This is the main scene of the test game.");
        public static readonly SceneDetail TestEndScene =
            new SceneDetail("Level/Experiment/Test/EndScene", "This is the scene scene of the test game.");

        private SceneDetail(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
