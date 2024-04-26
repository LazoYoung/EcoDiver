using UnityEngine;

namespace Script.Quest
{
    public class QuestB : MonoBehaviour, IQuest
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
            return Input.GetKeyDown(KeyCode.U);
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

        public void Complete()
        {
            Debug.Log("Quest B Completed");
        }

        public void Notify()
        {
            QuestObserver.Instance.UpdateQuest(this);
        }
    }
}
