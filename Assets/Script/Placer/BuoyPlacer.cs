using UnityEngine;

public class BuoyPlacer : MonoBehaviour
{
    public GameObject buoyPrefab; // 부표 프리팹
    public int numberOfBuoys = 10; // 배치할 부표의 총 개수
    public float radius = 5.0f; // 원의 반지름

    void Start()
    {
        PlaceBuoys();
    }

    void PlaceBuoys()
    {
        for (int i = 0; i < numberOfBuoys; i++)
        {
            // 각 부표의 각도 계산 (라디안으로 변환)
            float angle = i * Mathf.PI * 2 / numberOfBuoys;
            // 각도를 사용하여 x, y 위치 계산
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;
            // 부표의 최종 위치 계산
            Vector3 position = new Vector3(x, 0, y) + transform.position; // 원의 중심을 기준으로 함
            // 부표 인스턴스화 및 배치
            Instantiate(buoyPrefab, position, Quaternion.identity);
        }
    }
}