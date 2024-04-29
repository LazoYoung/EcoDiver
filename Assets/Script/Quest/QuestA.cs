using Script.Collect;
using UnityEngine;

namespace Script.Quest
{
    public class QuestA : MonoBehaviour, IQuest
    {
        private bool isCompleted = false;

        // 그룹 A의 요구 아이템 수
        private int requiredItemsInGroupA = 2;

        // 퀘스트 완료 시 재생될 사운드 클립
        public AudioClip completionSound;

        private void Start()
        {
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
            // 그룹 A의 아이템 수집 여부 확인
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
            // 메인 카메라에서 AudioSource 컴포넌트를 찾음
            AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
            if (audioSource != null && completionSound != null)
            {
                audioSource.PlayOneShot(completionSound);
            }
        }
    }
}