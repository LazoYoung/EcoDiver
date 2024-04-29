using UnityEngine;
using Crest;

public class WaterSoundManager : MonoBehaviour
{
    public OceanRenderer oceanRenderer; // Crest의 OceanRenderer 컴포넌트를 할당
    public AudioSource audioSource; // AudioSource 컴포넌트를 할당
    public AudioClip divingSound; // 다이빙 사운드
    public AudioClip cameOutSound; // 물에서 나오는 사운드
    public AudioClip underwaterLoopSound; // 물속 루프 사운드
    public AudioClip surfaceLoopSound; // 물 위 루프 사운드

    private bool isUnderwater = false; // 현재 물속인지 여부
    private SampleHeightHelper _sampleHeightHelper; // 높이 샘플링을 위한 헬퍼

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
                    audioSource.clip = underwaterLoopSound; // 루프 사운드를 물속 사운드로 변경
                }
                else
                {
                    audioSource.PlayOneShot(cameOutSound);
                    audioSource.clip = surfaceLoopSound; // 루프 사운드를 물 위 사운드로 변경
                }
                audioSource.Play(); // 새로운 루프 사운드 재생
            }
        }
    }
}
