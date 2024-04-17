using UnityEngine;

public interface IQuest
{
    // This method is called when the quest is completed
    void Complete();

    // This method is used to notify observers that something has changed
    void Notify();

    void Activate();
    void Deactivate();

    Transform GetTransform();
}
