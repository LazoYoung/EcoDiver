using UnityEngine;


namespace Script.Quest
{
    public abstract class Quest : MonoBehaviour
    {
        [SerializeField] private AudioClip completionSound;
        [SerializeField] private AudioSource audioSource;
        
        public abstract bool CanComplete();

        public virtual void OnComplete()
        {
            Debug.Log($"{GetQuestName()} Completed");
            PlayCompletionSound();
        }

        public void Notify()
        {
            QuestObserver.Instance.UpdateQuest(this);
        }

        public virtual void Activate()
        {
            Debug.Log($"{GetQuestName()} Activated");
            gameObject.SetActive(true);
        }

        public virtual void Deactivate()
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

        private void PlayCompletionSound()
        {
            if (audioSource != null && completionSound != null)
            {
                audioSource.PlayOneShot(completionSound);
            }
        }
    }
}
