using UnityEngine;

public class RandomObjectsPlacerWithY : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPrefabSettings
    {
        public GameObject prefab; // ������Ʈ ������
        public int number; // ������ ������Ʈ�� ��
        public Vector2 scaleRange = new Vector2(1.0f, 1.5f); // ������Ʈ ũ�� ���� (�ּ�, �ִ�)
    }

    public ObjectPrefabSettings[] objectPrefabsSettings; // ������Ʈ ���� �迭
    public Vector3 areaSize = new Vector3(50, 50, 50); // ��ġ�� ������ ũ��, Y �൵ �����Ͽ� ����
    public Vector3 areaPositionOffset = Vector3.zero; // ��ġ ������ ��ġ ����

    void Start()
    {
        PlaceObjectsRandomly();
    }

    void PlaceObjectsRandomly()
    {
        foreach (var setting in objectPrefabsSettings)
        {
            for (int i = 0; i < setting.number; i++)
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(-areaSize.x / 2, areaSize.x / 2),
                    Random.Range(-areaSize.y / 2, areaSize.y / 2), // Y���� ���� ���� ��ġ ����
                    Random.Range(-areaSize.z / 2, areaSize.z / 2)
                ) + transform.position + areaPositionOffset;

                float randomScale = Random.Range(setting.scaleRange.x, setting.scaleRange.y);

                // �⺻ �������� 1/10000�� ����
                randomScale *= 0.0001f;

                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                GameObject objectInstance = Instantiate(setting.prefab, randomPosition, randomRotation);
                objectInstance.transform.SetParent(transform);
                objectInstance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position + areaPositionOffset, new Vector3(areaSize.x, 1, areaSize.z));
    }
}
