using System.Collections.Generic;
using UnityEngine;

public class QuestObserver: MonoBehaviour
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
        registerQuest(FindObjectOfType<QuestA>());
        registerQuest(FindObjectOfType<QuestB>());
        registerQuest(FindObjectOfType<QuestC>());

        StartQuest();
    }

    private void registerQuest(IQuest quest)
    {
        quest.Deactivate();
        _quests.Enqueue(quest);
    }

    private IQuest completeAndPeekNext()
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
        var nextQuest = completeAndPeekNext();
        if (nextQuest == null)
        {
            //ending 처리 or nextQuest가 엔딩인 경우를 만들기.
        }

        completeAction();
        StartQuest();
    }

    private void StartQuest()
    {
        if(_quests.Count == 0)
        {
            ending();
            return;
        }
        var quest = _quests.Peek();
        Debug.Log("Starting Quest: " + quest);
        quest.Activate();

        updateBackground(quest);

        updateMinimap(quest);
        updateStatusBar(quest);
        updateArrow(quest);
    }

    private void ending()
    {
        Debug.Log("No Quests");
        arrowIndicator.gameObject.SetActive(false);

    }

    private void updateArrow(IQuest nextQuest)
    {
        arrowIndicator.questTransform = nextQuest.GetTransform();
    }

    private void updateStatusBar(IQuest nextQuest)
    {
        //throw new System.NotImplementedException();
    }

    private void updateMinimap(IQuest nextQuest)
    {
        //throw new System.NotImplementedException();
    }

    private void updateBackground(IQuest nextQuest)
    {
        //throw new System.NotImplementedException();
    }

    private void completeAction()
    {
        //throw new System.NotImplementedException();
    }
}
