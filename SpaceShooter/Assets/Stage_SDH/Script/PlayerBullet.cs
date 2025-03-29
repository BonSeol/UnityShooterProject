using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // 총알의 속도와 피해 값
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 10;

    // 애니메이터와 리지드바디2D 컴포넌트
    protected Animator animator;
    protected Rigidbody2D rb;

    // 초기화 함수
    protected virtual void Start()
    {
        // 애니메이터와 리지드바디2D 컴포넌트 가져오기
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // 발사 애니메이션 재생
        animator.Play("Fire");

        // 일정 시간 후 이동 애니메이션으로 변경
        Invoke(nameof(StartMoving), 0.1f);

        // 총알이 발사 방향으로 이동하도록 설정
        rb.linearVelocity = transform.right * speed;
    }

    // 총알이 이동을 시작하면 호출되는 함수
    protected void StartMoving()
    {
        // 이동 애니메이션으로 변경
        animator.Play("Move");
    }

    // 충돌 처리 함수
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // 몬스터와 충돌한 경우
        if (collision.CompareTag("Monster"))
        {
            // 히트 애니메이션 재생
            animator.Play("Hit");

            // 충돌 후 총알 정지
            rb.linearVelocity = Vector2.zero;

            // 몬스터의 TakeDamage 함수 호출하여 피해 주기
            Monster monster = collision.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }

            // 0.2초 후 총알 객체를 파괴
            Destroy(gameObject, 0.2f);
        }

        // 벽과 충돌한 경우
        else if (collision.CompareTag("Wall"))
        {
            // 히트 애니메이션 재생
            animator.Play("Hit");

            // 충돌 후 총알 정지
            rb.linearVelocity = Vector2.zero;

            // 0.2초 후 총알 객체를 파괴
            Destroy(gameObject, 0.2f);
        }
    }
}
