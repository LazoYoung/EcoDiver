using Script.Collect;
using UnityEngine;

namespace Script.Quest
{
    public class QuestC : MonoBehaviour, IQuest
    {
        private bool isCompleted = false;
        private readonly string _questName = "Quest C";
        private readonly string _questDescription = "Press I to complete Quest C";

        // 그룹 C의 요구 아이템 수
        private int requiredItemsInGroupC = 5;

        // 퀘스트 완료 시 재생될 사운드 클립
        public AudioClip completionSound;

        private void Start()
        {
        }

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
            // 그룹 C의 아이템 수집 여부 확인
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
            // 메인 카메라에서 AudioSource 컴포넌트를 찾음
            AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
            if (audioSource != null && completionSound != null)
            {
                audioSource.PlayOneShot(completionSound);
            }
        }
    }
}