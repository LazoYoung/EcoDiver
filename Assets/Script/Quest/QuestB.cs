using UnityEngine;

namespace Script.Quest
{
    public class QuestB : MonoBehaviour, IQuest
    {
        private bool isCompleted = false;
        private readonly string _questName = "Quest B";
        private readonly string _questDescription = "Press U to complete Quest B";

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

        public void OnComplete()
        {
            Debug.Log("Quest B Completed");
        }

        public void Notify()
        {
            QuestObserver.Instance.UpdateQuest(this);
        }
    }
}
