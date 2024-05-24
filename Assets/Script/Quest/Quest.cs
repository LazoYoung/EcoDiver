using UnityEngine;

namespace Script.Quest
{
    public abstract class Quest : MonoBehaviour
    {
        [SerializeField] private AudioClip completionSound;
        [SerializeField] private AudioSource audioSource;
        private bool _isCompleted;

        protected abstract bool CanComplete();

        protected virtual void OnComplete()
        {
            Debug.Log($"{GetQuestName()} Completed");
            PlayCompletionSound();
        }

        public void Activate()
        {
            Debug.Log($"{GetQuestName()} Activated");
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            Debug.Log($"{GetQuestName()} Deactivated");
            gameObject.SetActive(false);
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public abstract string GetQuestName();
        
        public abstract string GetQuestDescription();
        
        protected virtual void Start()
        {
            if (audioSource == null)
            {
                audioSource = Camera.main?.GetComponent<AudioSource>();
            }
        }
        
        protected virtual void Update()
        {
            if (!_isCompleted && CanComplete())
            {
                _isCompleted = true;
                OnComplete();
                Notify();
            }
        }
        
        protected void SetupBoxCollider()
        {
            if (!TryGetComponent(out BoxCollider boxCollider))
            {
                boxCollider = gameObject.AddComponent<BoxCollider>();
            }

            boxCollider.isTrigger = true;
        }
        
        private void Notify()
        {
            QuestObserver.Instance.UpdateQuest(this);
        }

        private void PlayCompletionSound()
        {
            if (audioSource != null && completionSound != null)
            {
                audioSource.PlayOneShot(completionSound);
            }
        }
    }
}
