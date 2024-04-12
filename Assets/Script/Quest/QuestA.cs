using UnityEngine;

public class QuestA : MonoBehaviour, IQuest
{
    private void Start()
    {
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
        Notify();
    }

    public void Notify()
    {
        QuestObserver.Instance.Update(this);
    }
}
