using UnityEngine;


namespace Script.Quest
{
    public class QuestA : MonoBehaviour, IQuest
    {
        private bool isCompleted = false;

        private void Start()
        {
        }

        private void Update()
        {
            if (!isCompleted && isCompleteNow())
            {
                isCompleted = true;
                Complete();
                Notify();
            }
        }

        public bool isCompleteNow()
        {
            return Input.GetKeyDown(KeyCode.Y);
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

        public void Complete()
        {
            Debug.Log("Quest A Completed");
        }

        public void Notify()
        {
            QuestObserver.Instance.UpdateQuest(this);
        }
    }
}
