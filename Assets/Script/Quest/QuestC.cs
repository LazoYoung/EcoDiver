using Script.Environment.Collect;
using UnityEngine;

namespace Script.Quest
{
    public class QuestC : MonoBehaviour, IQuest
    {
        private bool isCompleted = false;
        private readonly string _questName = "Quest C";
        private readonly string _questDescription = "Press I to complete Quest C";

        private int requiredItemsInGroupC = 5;

        public AudioClip completionSound;

        public string GetQuestName()
        {
            return _questName;
        }

        public string GetQuestDescription()
        {
            return _questDescription;
        }

        private void Update()
        {
            if (!isCompleted && CanComplete())
            {
                isCompleted = true;
                OnComplete();
                Notify();
            }
        }

        public bool CanComplete()
        {
            return CollectManager.Instance.GetTotalCollectedItems() >= requiredItemsInGroupC;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void Activate()
        {
            Debug.Log("Quest C Activated");
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            Debug.Log("Quest C Deactivated");
            gameObject.SetActive(false);
        }

        public void OnComplete()
        {
            Debug.Log("Quest C Completed");
            PlayCompletionSound();
        }

        public void Notify()
        {
            QuestObserver.Instance.UpdateQuest(this);
        }

        private void PlayCompletionSound()
        {
            AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
            if (audioSource != null && completionSound != null)
            {
                audioSource.PlayOneShot(completionSound);
            }
        }
    }
}