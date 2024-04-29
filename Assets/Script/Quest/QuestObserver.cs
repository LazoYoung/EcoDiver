using System.Collections.Generic;
using UnityEngine;

namespace Script.Quest
{
    public class QuestObserver : MonoBehaviour
    {
        public static QuestObserver Instance { get; private set; } = null;
        private Queue<IQuest> _quests = new Queue<IQuest>();

        public QuestArrowIndicator arrowIndicator;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            RegisterQuest(FindObjectOfType<QuestA>());
            RegisterQuest(FindObjectOfType<QuestB>());
            RegisterQuest(FindObjectOfType<QuestC>());

            StartQuest();
        }

        private void RegisterQuest(IQuest quest)
        {
            quest.Deactivate();
            _quests.Enqueue(quest);
        }

        private IQuest CompleteAndPeekNext()
        {
            _quests.Peek().Deactivate();
            if (_quests.Count == 0)
            {
                return null;
            }

            return _quests.Dequeue();
        }

        public void UpdateQuest(IQuest quest)
        {
            if (quest != _quests.Peek())
            {
                Debug.LogError("Updated quest is not the current quest.");
                return;
            }

            var nextQuest = CompleteAndPeekNext();
            if (nextQuest == null)
            {
                // 모든 퀘스트를 완료한 경우의 처리
                OnFinishAllQuests();
                return;
            }
            
            OnCompleteQuest();
            StartQuest();
        }

        private void StartQuest()
        {
            if (_quests.Count == 0)
            {
                OnFinishAllQuests();
                return;
            }

            var quest = _quests.Peek();
            Debug.Log("Starting Quest: " + quest);
            quest.Activate();

            UpdateBackground(quest);

            UpdateMinimap(quest);
            UpdateStatusBar(quest);
            UpdateArrow(quest);
        }

        private void OnFinishAllQuests()
        {
            Debug.Log("No Quests");
            arrowIndicator.gameObject.SetActive(false);
        }

        private void UpdateArrow(IQuest nextQuest)
        {
            arrowIndicator.questTransform = nextQuest.GetTransform();
        }

        private void UpdateStatusBar(IQuest nextQuest)
        {
            //throw new System.NotImplementedException();
        }

        private void UpdateMinimap(IQuest nextQuest)
        {
            //throw new System.NotImplementedException();
        }

        private void UpdateBackground(IQuest nextQuest)
        {
            //throw new System.NotImplementedException();
        }

        private void OnCompleteQuest()
        {
            //throw new System.NotImplementedException();
        }
    }
}
