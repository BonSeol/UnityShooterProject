using UnityEngine;
using UnityEngine.InputSystem;

public class Altar : MonoBehaviour
{
    [SerializeField] private GameObject flameObject; // 불꽃 오브젝트
    [SerializeField] private AudioClip altarSound; // 재단 활성화 사운드
    private AudioSource audioSource;
    private bool isPlayerinside_Object = false; // 오브젝트 구역에 들어갔는지 확인하는 변수

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // 불꽃 활성화 함수
    public void ActivateFlame()
    {
        if (flameObject != null)
            flameObject.SetActive(true); // 불꽃 오브젝트 활성화
    }

    void OnInteract(InputValue value)
    {
        // 오브젝트 구역에 들어왔는지체크
        if (isPlayerinside_Object)
        {
            if (GameManager.Instance.gemCount > 0)
            {
                ActivateFlame();
                GameManager.Instance.AltarCount++;
                GameManager.Instance.gemCount--;
                isPlayerinside_Object = false;
                audioSource.PlayOneShot(altarSound);
                Destroy(gameObject, 1.5f);
            }
        }
    }

    // 트리거 영역에 들어갈 때 호출됩니다.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // 특정 태그(Gem, Altar, Door)에 들어갔는지 확인
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