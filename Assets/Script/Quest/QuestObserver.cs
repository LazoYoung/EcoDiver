using System.Collections.Generic;
using Script.Display;
using UnityEngine;

namespace Script.Quest
{
    public class QuestObserver : MonoBehaviour
    {
        public static QuestObserver Instance { get; private set; }
        
        [SerializeField]
        private QuestArrowIndicator arrowIndicator;

        [SerializeField]
        private List<Quest> quests;
        
        private readonly Queue<Quest> _quests = new Queue<Quest>();
        private QuestLevel _questLevel;
        private Quest _currentQuest;

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
            foreach (var quest in quests)
            {
                if (quest != null)
                {
                    RegisterQuest(quest);
                }
            }

            StartQuest();
            _questLevel = new QuestLevel(_quests.Count);
            UpdateDisplay();
        }

        private void RegisterQuest(Quest quest)
        {
            quest.Deactivate();
            _quests.Enqueue(quest);
        }

        private Quest CompleteAndPeekNext()
        {
            _quests.Peek().Deactivate();
            if (_quests.Count == 0)
            {
                return null;
            }
            return _quests.Dequeue();
        }

        public void UpdateQuest(Quest quest)
        {
            _questLevel.LevelUp();

            var nextQuest = CompleteAndPeekNext();
            if (nextQuest == null)
            {
                // todo: finale if required
                UpdateDisplay();
            }

            OnCompleteQuest();
            StartQuest();
            UpdateDisplay();
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
            Debug.Log("All Quests Complete");
            arrowIndicator.gameObject.SetActive(false);
        }

        private void UpdateArrow(Quest nextQuest)
        {
            arrowIndicator.questTransform = nextQuest.GetTransform();
        }

        private void UpdateStatusBar(Quest nextQuest)
        {
            // todo: method stub
        }

        private void UpdateMinimap(Quest nextQuest)
        {
            // todo: implement if required
        }

        private void UpdateBackground(Quest nextQuest)
        {
            // todo: implement if required
        }

        private void OnCompleteQuest()
        {
            // todo: implement if required
        }

        private void UpdateDisplay()
        {
            var display = DisplayManager.Instance;
            display.QuestLevel = _questLevel;
            display.QuestName = _currentQuest?.GetQuestName();
            display.QuestDescription = _currentQuest?.GetQuestDescription();
        }
    }
}
