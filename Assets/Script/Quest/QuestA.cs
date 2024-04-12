using UnityEngine;

public class QuestA : Quest
{
    private QuestObserver _observer;

    public void Start()
    {

    }
    // Concrete implementation of completing the quest
    public void Complete()
    {
        Debug.Log("Quest A is complete!");
        Notify();
    }

    // Concrete implementation of the Notify method
    public void Notify()
    {
        _observer.Update(this);
    }
}
