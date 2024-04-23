using UnityEngine;

namespace Script.Quest
{
    public class QuestC : MonoBehaviour, IQuest
    {
        private void Start()
        {
        }

        private void Update()
        {
            // Check if the Y key was pressed this frame
            if (Input.GetKeyDown(KeyCode.I))
            {
                Complete();
            }
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
            Notify();
        }

        public void Notify()
        {
            QuestObserver.Instance.UpdateQuest(this);
        }
    }
}
