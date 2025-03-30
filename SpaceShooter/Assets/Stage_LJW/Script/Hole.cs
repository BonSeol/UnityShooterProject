using UnityEngine;

public class Hole : MonoBehaviour
{
    public int damage = 1;
    public float visibleDuration = 1f;

    void Start()
    {
        // 해당 위치에서 고정적으로 애니메이션 재생 후 사라지게
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play(0); // 첫 번째 상태(애니메이션) 자동 재생
        }

        Destroy(gameObject, visibleDuration); // 시간 지나면 삭제
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 데미지를 준다면 여기에 Player에 damage 주는 코드
            Debug.Log("Player hit by hole");
        }
    }

    private void OnBecameInvisible()
    {
        // 혹시 카메라 밖으로 나갔을 경우도 삭제
        Destroy(gameObject);
    }
}
