using UnityEngine;
using Crest;

public class WaterSoundManager : MonoBehaviour
{
    public OceanRenderer oceanRenderer; // Crest�� OceanRenderer ������Ʈ�� �Ҵ�
    public AudioSource audioSource; // AudioSource ������Ʈ�� �Ҵ�
    public AudioClip divingSound; // ���̺� ����
    public AudioClip cameOutSound; // ������ ������ ����
    public AudioClip underwaterLoopSound; // ���� ���� ����
    public AudioClip surfaceLoopSound; // �� �� ���� ����

    private bool isUnderwater = false; // ���� �������� ����
    private SampleHeightHelper _sampleHeightHelper; // ���� ���ø��� ���� ����

    private void Start()
    {
        _sampleHeightHelper = new SampleHeightHelper();
        audioSource.clip = surfaceLoopSound;
        audioSource.Play();
    }

    private void Update()
    {
        _sampleHeightHelper.Init(Camera.main.transform.position, 0f);

        if (_sampleHeightHelper.Sample(out float waterHeight))
        {
            float cameraHeight = Camera.main.transform.position.y;
            bool currentlyUnderwater = cameraHeight < waterHeight;

            if (isUnderwater != currentlyUnderwater)
            {
                isUnderwater = currentlyUnderwater;

                if (isUnderwater)
                {
                    audioSource.PlayOneShot(divingSound);
                    audioSource.clip = underwaterLoopSound; // ���� ���带 ���� ����� ����
                }
                else
                {
                    audioSource.PlayOneShot(cameOutSound);
                    audioSource.clip = surfaceLoopSound; // ���� ���带 �� �� ����� ����
                }
                audioSource.Play(); // ���ο� ���� ���� ���
            }
        }
    }
}
