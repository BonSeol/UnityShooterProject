using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public GameObject target;
    public float Speed = 1f;
    public int HP = 30;
    public int damageToPlayer = 3;
    public float attackRange = 1.5f; // 공격 거리
    public float stopRange = 1.2f; // 공격 전에 멈추는 거리
    public float attackInterval = 1.5f; // 공격 간격

    public Vector2 dir;
    public Vector2 dirNo;
    Animator animator;

    Rigidbody2D rb;
    SpriteRenderer sp;
    private bool isDead = false;
    private bool isAttacking = false;
    private bool isHit = false;
    private int isRight = 1;
    private float attackTimer = 0f; // 공격 타이머


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (isDead) return;

        attackTimer -= Time.deltaTime; // 공격 타이머 감소

        dir = target.transform.position - transform.position;
        dirNo = dir.normalized;

        float distanceToPlayer = dir.magnitude;


        if (distanceToPlayer > stopRange)
        {
            rb.linearVelocity = dir.normalized * Speed; // 플레이어를 향해 이동
            animator.SetBool("isWalking", true);
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // 멈춤
            animator.SetBool("isWalking", false);
        }


        if (distanceToPlayer <= attackRange && attackTimer <= 0f)
        {
            Attack();
        }

        // 방향 조절
        if ((dir.x < 0 && isRight == 1) || (dir.x > 0 && isRight == -1))
        {
            sp.flipX = sp.flipX == true ? false : true;
            isRight = -isRight;
        }

        // transform.Translate(dir * Speed * Time.deltaTime);
    }

    private void Attack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero; // 이동 중지
        animator.SetTrigger("Attack1"); // 공격 애니메이션 실행
        attackTimer = attackInterval; // 공격 타이머 리셋
    }

    public void PerformAttack()
    {
        if (Vector2.Distance(transform.position, target.transform.position) <= attackRange)
        {
            //target.GetComponent<Player>().TakeDamage(1);
        }
    }

    // 애니메이션 이벤트: 공격 종료 후 이동 재개
    public void EndAttack()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // collision.GetComponent<Player>().TakeDamage(damageToPlayer);
        }
        else if (collision.CompareTag("Bullet"))
        {
            TakeDamage(10);
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        HP -= damage;
        animator.SetTrigger("Hit");

        if (HP <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("Death", true);
        rb.linearVelocity = Vector2.zero; // 죽을 때 이동 정지
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }

    //디버깅용 기즈모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopRange);
    }
}