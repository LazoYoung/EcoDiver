using UnityEngine;

public class BuoyPlacer : MonoBehaviour
{
    public GameObject buoyPrefab; // ��ǥ ������
    public int numberOfBuoys = 10; // ��ġ�� ��ǥ�� �� ����
    public float radius = 5.0f; // ���� ������

    void Start()
    {
        PlaceBuoys();
    }

    void PlaceBuoys()
    {
        for (int i = 0; i < numberOfBuoys; i++)
        {
            // �� ��ǥ�� ���� ��� (�������� ��ȯ)
            float angle = i * Mathf.PI * 2 / numberOfBuoys;
            // ������ ����Ͽ� x, y ��ġ ���
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;
            // ��ǥ�� ���� ��ġ ���
            Vector3 position = new Vector3(x, 0, y) + transform.position; // ���� �߽��� �������� ��
            // ��ǥ �ν��Ͻ�ȭ �� ��ġ
            Instantiate(buoyPrefab, position, Quaternion.identity);
        }
    }
}