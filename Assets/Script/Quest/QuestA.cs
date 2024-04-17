using UnityEngine;

public class QuestA : MonoBehaviour, IQuest
{
    private void Start()
    {
    }

    private void Update()
    {
        // Check if the Y key was pressed this frame
        if (Input.GetKeyDown(KeyCode.Y))
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
        QuestObserver.Instance.UpdateQuest(this);
    }
}
