using System.Collections.Generic;
using Script.Display;
using UnityEngine;

namespace Script.Quest
{
    public class QuestObserver : MonoBehaviour
    {
        public static QuestObserver Instance { get; private set; } = null;
        private readonly Queue<IQuest> _quests = new Queue<IQuest>();
        private QuestLevel _questLevel;
        private IQuest _currentQuest;
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
            _questLevel = new QuestLevel(_quests.Count);
            updateDisplay();
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
            _questLevel.LevelUp();
            updateDisplay();

            var nextQuest = CompleteAndPeekNext();
            if (nextQuest == null)
            {
                //ending 처리 or nextQuest가 엔딩인 경우를 만들기.
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

            _currentQuest = _quests.Peek();

            Debug.Log("Starting Quest: " + _currentQuest);
            _currentQuest.Activate();

            UpdateBackground(_currentQuest);

            UpdateMinimap(_currentQuest);
            UpdateStatusBar(_currentQuest);
            UpdateArrow(_currentQuest);
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

        private void updateDisplay()
        {
            DisplayManager.Instance.QuestLevel = _questLevel;
            DisplayManager.Instance.QuestName = _currentQuest.GetQuestName();
            DisplayManager.Instance.QuestDescription = _currentQuest.GetQuestDescription();
        }
    }
}
