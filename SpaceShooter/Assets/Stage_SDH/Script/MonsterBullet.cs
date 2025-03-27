using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    public float lifeTime = 5f;  // 총알의 생명 시간
    Rigidbody2D rb;
    public float speed = 10f;
    public int damage = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 총알이 발사 방향으로 이동하도록 설정
        rb.linearVelocity = transform.right * speed;

        // 일정 시간 후 총알 삭제
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 총알이 충돌한 물체에 대해 처리 (예: 데미지 주기)
        if (collision.CompareTag("Player"))
        {
            rb.linearVelocity = Vector2.zero;

            // Player의 TakeDamage 호출
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(10); // 기본 데미지 10
            }

            // 총알이 무엇인가에 충돌하면 총알 삭제
            Destroy(gameObject, 0.2f);
        }

        // 벽에 맞았을 경우
        else if (collision.CompareTag("Wall"))
        {
            rb.linearVelocity = Vector2.zero;

            // 애니메이션 끝난 후 삭제
            Destroy(gameObject, 0.2f);
        }
    }
}
