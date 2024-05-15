using System.Collections.Generic;

namespace Scene
{
    public struct SceneDetail
    {
        public readonly string Name;
        public readonly string Description;

        //Production Scenes
        public static readonly SceneDetail StartScene =
            new SceneDetail("Level/Production/StartScene", "This is the starting scene of the game.");
        public static readonly SceneDetail MainScene =
            new SceneDetail("Level/Production/MainScene", "This is the main scene of the game.");
        public static readonly SceneDetail EndScene =
            new SceneDetail("Level/Production/EndScene", "This is the ending scene of the game.");

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
