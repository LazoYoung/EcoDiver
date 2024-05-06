using UnityEngine;

namespace Script.Environment.Placer
{
    public class BuoyPlacer : MonoBehaviour
    {
        public GameObject buoyPrefab;
        public int numberOfBuoys = 10;
        public float radius = 5.0f;

        void Start()
        {
            PlaceBuoys();
        }

        void PlaceBuoys()
        {
            for (int i = 0; i < numberOfBuoys; i++)
            {
                float angle = i * Mathf.PI * 2 / numberOfBuoys;
                float x = Mathf.Sin(angle) * radius;
                float y = Mathf.Cos(angle) * radius;
                Vector3 position = new Vector3(x, 0, y) + transform.position; // ���� �߽��� �������� ��
                Instantiate(buoyPrefab, position, Quaternion.identity);
            }
        }
    }
}