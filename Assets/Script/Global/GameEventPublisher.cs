using UnityEngine;

namespace Script.Global
{
    public class GameEventPublisher : MonoBehaviour
    {
        public delegate void GameEvent(GameEventType eGameEventType);
        public static event GameEvent OnGameEvent;

        void Update()
        {
            //If press P key, pause the game
            if (Input.GetKeyDown(KeyCode.P))
            {
                PublishGameEvent(GameEventType.PAUSE);
            }
            //If press R key, resume the game
            if (Input.GetKeyDown(KeyCode.R))
            {
                PublishGameEvent(GameEventType.RESUME);
            }
        }

        private static void PublishGameEvent(GameEventType eGameEventType)
        {
            Debug.Log("Publishing event: " + eGameEventType);
            OnGameEvent?.Invoke(eGameEventType);
        }

    }
}
