using UnityEngine;
using UnityEngine.Serialization;


namespace Script.Display
{
    public class OxygenManager : Global.GameEventSubscriber
    {
        private const float MaxOxygenPercent = 100f; // Maximum oxygen capacity

        [SerializeField] [Tooltip("consume oxygen if player is not in water.")]
        private bool consumeOxygenMode = false;
        [SerializeField] [Tooltip("seconds Oxygen consume 0.1 percent.")]
        private int oxygenConsumptionSecond = 10; // Oxygen consumed per second at normal conditions

        private float _currentOxygen; // Current oxygen level
        private float _oxygenTimer = 0;

        void Start()
        {
            OnEnable();
            _currentOxygen = MaxOxygenPercent;
        }

        void Update()
        {
            if (doConsumeOxygen())
            {
                _oxygenTimer += Time.deltaTime;
                if (_oxygenTimer >= oxygenConsumptionSecond)
                {
                    // Consume 1% of oxygen and reset the timer
                    ConsumeOxygen(0.1f);
                    _oxygenTimer = 0; // Reset timer after consuming oxygen
                }
            }
        }

        protected override void HandleGameEvent(Global.GameEventType eGameEventType)
        {
            switch (eGameEventType)
            {
                case Global.GameEventType.PAUSE:
                    Pause();
                    break;
                case Global.GameEventType.RESUME:
                    Resume();
                    break;
            }
        }

        private void Pause()
        {
            enabled = false;
        }

        private void Resume()
        {
            enabled = true;
        }

        public void FulfillOxygen()
        {
            _currentOxygen = MaxOxygenPercent;
            _oxygenTimer = 0;
        }

        private void ConsumeOxygen(float amount)
        {
            _currentOxygen -= amount;
            if (_currentOxygen <= 0)
            {
                _currentOxygen = 0;
                Debug.Log("Oxygen depleted!");
                // Implement death or severe consequence logic here
            }
            else
            {
                Debug.Log($"Oxygen level: {_currentOxygen}%");
            }
        }

        private bool doConsumeOxygen()
        {
            if (consumeOxygenMode)
            {
                return true;
            }

            // TODO 바다 닿는지 여부에 따라 달라지도록.
            return false;
        }
    }
}
