using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;

    [Header("References")]
    public Animator animator;
    public Transform player;
    private Rigidbody2D rb;

    [Header("Animation Triggers")]
    private const string ATTACK1_ANIM = "Attack1";
    private const string ATTACK2_ANIM = "Attack2";
    private const string HIT_ANIM = "Hit";
    private const string DEATH_ANIM = "Death";

    private bool isAttacking = false;
    private bool canMove = true;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (canMove && player != null)
        {
            FollowPlayer();
        }

        CheckAttackRange();
    }

    void FollowPlayer()
    {
        // 플레이어 방향으로 이동
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        // 모든 애니메이션 상태 false로 설정 (Idle 상태로)
        animator.SetBool(ATTACK1_ANIM, false);
        animator.SetBool(ATTACK2_ANIM, false);
        animator.SetBool(HIT_ANIM, false);
        animator.SetBool(DEATH_ANIM, false);
    }

    void CheckAttackRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            StartCoroutine(AttackPlayer(distanceToPlayer));
        }
    }

    IEnumerator AttackPlayer(float distance)
    {
        isAttacking = true;
        canMove = false;

        // 모든 애니메이션 비활성화
        animator.SetBool(ATTACK1_ANIM, false);
        animator.SetBool(ATTACK2_ANIM, false);
        animator.SetBool(HIT_ANIM, false);
        animator.SetBool(DEATH_ANIM, false);

        // 가까운 거리 공격
        if (distance <= 1.5f)
        {
            animator.SetBool(ATTACK1_ANIM, true);
        }
        // HP 50 이하일 때 원거리 공격
        else if (currentHealth <= 50)
        {
            animator.SetBool(ATTACK2_ANIM, true);
        }

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        canMove = true;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // 모든 애니메이션 비활성화
        animator.SetBool(ATTACK1_ANIM, false);
        animator.SetBool(ATTACK2_ANIM, false);
        animator.SetBool(DEATH_ANIM, false);

        // Hit 애니메이션 활성화
        animator.SetBool(HIT_ANIM, true);

        // HP가 0 이하면 Death 애니메이션
        if (currentHealth <= 0)
        {
            animator.SetBool(DEATH_ANIM, true);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 총알과 충돌 시 처리
        if (collision.gameObject.CompareTag("Bullet"))
        {
            float Damage = collision.gameObject.GetComponent<PBullet>().damage;
            TakeDamage(Damage);
        }
    }
}