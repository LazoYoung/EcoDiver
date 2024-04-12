using UnityEngine;

public class QuestB : MonoBehaviour, IQuest
{
    private void Start()
    {
    }

    private void Update()
    {
        // Check if the Y key was pressed this frame
        if (Input.GetKeyDown(KeyCode.U))
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
        Debug.Log("Quest B Activated");
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        Debug.Log("Quest B Deactivated");
        gameObject.SetActive(false);
    }

    public void Complete()
    {
        Debug.Log("Quest B Completed");
        Notify();
    }

    public void Notify()
    {
        QuestObserver.Instance.UpdateQuest(this);
    }
}
