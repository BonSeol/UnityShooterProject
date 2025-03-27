using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource audioSource; // ����� �ҽ�
    public AudioClip[] audioClips;  // ���� ���� BGM
    int currentIndex = 0; // ���� ��� ���� Ʈ�� �ε���

    void Start()
    {
        if (audioClips.Length > 0)
        {
            PlayNextTrack();
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying) // ���� Ʈ���� ������ ���� Ʈ�� ���
        {
            PlayNextTrack();
        }
    }

    void PlayNextTrack()
    {
        if (audioClips.Length == 0) return;

        audioSource.clip = audioClips[currentIndex];
        audioSource.Play();

        currentIndex = (currentIndex + 1) % audioClips.Length; // ���� Ʈ�� (��ȯ)
    }
}
