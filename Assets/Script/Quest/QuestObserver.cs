using System.Collections.Generic;

public class QuestObserver
{
    private Queue<Quest> _quests = new Queue<Quest>(); //초기화 방법?

    private Quest peekNextQuest()
    {
        if (_quests.Count == 0)
        {
            return null;
        }
        return _quests.Dequeue();
    }

    public void Update(Quest quest)
    {
        var nextQuest = peekNextQuest();
        if (nextQuest == null)
        {
            //ending 처리 or nextQuest가 엔딩인 경우를 만들기.
        }

        completeAction();

        updateBackground(nextQuest);

        updateMinimap(nextQuest);
        updateStatusBar(nextQuest);
        updateArrow(nextQuest);
    }

    private void updateArrow(Quest nextQuest)
    {
        //throw new System.NotImplementedException();
    }

    private void updateStatusBar(Quest nextQuest)
    {
        //throw new System.NotImplementedException();
    }

    private void updateMinimap(Quest nextQuest)
    {
        //throw new System.NotImplementedException();
    }

    private void updateBackground(Quest nextQuest)
    {
        //throw new System.NotImplementedException();
    }

    private void completeAction()
    {
        //throw new System.NotImplementedException();
    }
}
