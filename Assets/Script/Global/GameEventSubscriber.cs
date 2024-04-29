using UnityEngine;

namespace Script.Global
{
    public abstract class GameEventSubscriber : MonoBehaviour
    {
        protected abstract void HandleGameEvent(GameEventType eGameEventType);

        public void OnEnable()
        {
            GameEventPublisher.OnGameEvent += HandleGameEvent;
        }

        public void OnDisable()
        {
            GameEventPublisher.OnGameEvent -= HandleGameEvent;
        }
    }
}
