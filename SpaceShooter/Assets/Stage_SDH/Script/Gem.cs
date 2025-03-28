using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Gem : MonoBehaviour
{
    [SerializeField] private AudioClip gemPickupSound; // Gem 획득 사운드
    private AudioSource audioSource;
    private bool isPlayerinside_Object = false; // 오브젝트 구역에 들어갔는지 확인하는 변수

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnInteract(InputValue value)
    {
        // 오브젝트 구역에 들어왔는지 체크
        if (isPlayerinside_Object)
        {
            GameManager.Instance.gemCount++; // 싱글톤을 통해 GameManager에 접근
            isPlayerinside_Object = false;

            // 소리가 설정되어 있다면 재생
            if (gemPickupSound != null)
                audioSource.PlayOneShot(gemPickupSound);
     
            Destroy(gameObject, 0.2f);
        }
    }

    // 트리거 영역에 들어갈 때 호출됩니다.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerinside_Object = true;
        }
    }

    // 벗어났는지 확인
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerinside_Object = false;
        }
    }
}
