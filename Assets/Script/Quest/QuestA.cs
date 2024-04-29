using Script.Collect;
using UnityEngine;

namespace Script.Quest
{
    public class QuestA : MonoBehaviour, IQuest
    {
        private bool isCompleted = false;

        // �׷� A�� �䱸 ������ ��
        private int requiredItemsInGroupA = 2;

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
            // �׷� A�� ������ ���� ���� Ȯ��
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
            // ���� ī�޶󿡼� AudioSource ������Ʈ�� ã��
            AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
            if (audioSource != null && completionSound != null)
            {
                audioSource.PlayOneShot(completionSound);
            }
        }
    }
}