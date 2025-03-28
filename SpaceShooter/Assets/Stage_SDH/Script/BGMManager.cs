using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // 오디오 소스
    [SerializeField] private AudioClip[] audioClips;  // BGM
    int currentIndex = 0; // 현재 재생 중인 트랙 인덱스

    void Start()
    {
        if (audioClips.Length > 0)
            PlayNextTrack();
    }

    void Update()
    {
        if (!audioSource.isPlaying) // 현재 트랙이 끝나면 다음 트랙 재생
            PlayNextTrack();
    }

    void PlayNextTrack()
    {
        if (audioClips.Length == 0) return;

        audioSource.clip = audioClips[currentIndex];
        audioSource.Play();

        currentIndex = (currentIndex + 1) % audioClips.Length; // 다음 트랙 (순환)
    }
}