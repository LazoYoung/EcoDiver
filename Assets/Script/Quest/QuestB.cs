using Script.Collect;
using UnityEngine;

namespace Script.Quest
{
    public class QuestB : MonoBehaviour, IQuest
    {
        private bool isCompleted = false;

        // �׷� B�� �䱸 ������ ��
        private int requiredItemsInGroupB = 4;

        // ����Ʈ �Ϸ� �� ����� ���� Ŭ��
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
            // �׷� B�� ������ ���� ���� Ȯ��
            return CollectManager.Instance.GetTotalCollectedItems() >= requiredItemsInGroupB;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void Activate()
        {
            Debug.Log("Quest B Activated");
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            Debug.Log("Quest B Deactivated");
            gameObject.SetActive(false);
        }

        public void OnComplete()
        {
            Debug.Log("Quest B Completed");
            PlayCompletionSound();
        }

        public void Notify()
        {
            QuestObserver.Instance.UpdateQuest(this);
        }

        private void PlayCompletionSound()
        {
            // ���� ī�޶󿡼� AudioSource ������Ʈ�� ã��
            AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
            if (audioSource != null && completionSound != null)
            {
                audioSource.PlayOneShot(completionSound);
            }
        }
    }
}