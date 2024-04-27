using UnityEngine;

public class RandomObjectsPlacer : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPrefabSettings
    {
        public GameObject prefab; // 오브젝트 프리팹
        public int number; // 생성할 오브젝트의 수
        public Vector2 scaleRange = new Vector2(1.0f, 1.5f); // 오브젝트 크기 범위 (최소, 최대)
    }

    public ObjectPrefabSettings[] objectPrefabsSettings; // 오브젝트 설정 배열
    public Vector3 areaSize = new Vector3(50, 0, 50); // 배치할 지역의 크기
    public Vector3 areaPositionOffset = Vector3.zero; // 배치 영역의 위치 조정

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
                    0,
                    Random.Range(-areaSize.z / 2, areaSize.z / 2)
                ) + transform.position + areaPositionOffset;

                if (Physics.Raycast(randomPosition + Vector3.up * 50, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                {
                    randomPosition.y = hit.point.y;
                }

                float randomScale = Random.Range(setting.scaleRange.x, setting.scaleRange.y);

                // 기본 스케일을 1/10000로 조정
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