using UnityEngine;


namespace Script.Quest
{
    public interface IQuest
    {
        // 퀘스트의 완료 조건을 달성했는지 여부를 반환.
        bool CanComplete();
        // 퀘스트를 완료했을 때의 동작.
        void Complete();

        // This method is used to notify observers that something has changed
        void Notify();

        void Activate();
        void Deactivate();

        Transform GetTransform();
    }
}
