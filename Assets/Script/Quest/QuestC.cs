using UnityEngine;

namespace Script.Quest
{
    public class QuestC : MonoBehaviour, IQuest
    {
        private bool isCompleted = false;

        private void Start()
        {
        }

        private void Update()
        {
            if (!isCompleted && CanComplete())
            {
                isCompleted = true;
                Complete();
                Notify();
            }
        }

        public bool CanComplete()
        {
            return Input.GetKeyDown(KeyCode.I);
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

        public void Complete()
        {
            Debug.Log("Quest C Completed");
        }

        public void Notify()
        {
            QuestObserver.Instance.UpdateQuest(this);
        }
    }
}
