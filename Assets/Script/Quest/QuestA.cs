using Script.Environment.Collect;
using UnityEngine;

namespace Script.Quest
{
    public class QuestA : MonoBehaviour, IQuest
    {
        private bool isCompleted = false;
        private readonly string _questName = "Quest A";
        private readonly string _questDescription = "Press Y to complete Quest A";

        private int requiredItemsInGroupA = 2;

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
            return CollectManager.Instance.GetTotalCollectedItems() >= requiredItemsInGroupA;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void Activate()
        {
            Debug.Log("Quest A Activated");
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            Debug.Log("Quest A Deactivated");
            gameObject.SetActive(false);
        }

        public void OnComplete()
        {
            Debug.Log("Quest A Completed");
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