using UnityEngine;
using System.Collections;


public class Monster_Golem : MonoBehaviour
{
    public float moveSpeed = 2f; // 이동 속도
    public float detectionRange = 10f; // 플레이어 감지 범위
    public float attackRange = 3f; // 공격 범위
    public float attackCooldown = 1f; // 공격 쿨타임
    public float maxHealth = 5; // 최대 체력
    public float attackDamage = 1f; // 공격력 
    public Transform attackPoint; // 공격 시작지점


    private Transform player;
    private float attackTimer;
    private float currentHealth;
    private Animator animator;
    private Rigidbody2D rb;


    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }


        if (Vector3.Distance(transform.position, player.position) <= detectionRange) //플레이어 감지 범위 내
        {
            LookAtPlayer();
            FollowPlayer();
        }
        else //플레이어 감지 범위 밖
        {
            Idle(); // ⭐ 플레이어 감지하지 못하면 가만히 있음
        }


    }

    private void Idle()
    {
        rb.linearVelocity = Vector2.zero; // 이동 정지
        animator.SetBool("walk", false);
        animator.SetBool("idle", true); // Idle 애니메이션 활성화
    }


    private void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        if (WallAhead(direction))
        {
            direction = FindAlternativeDirection(direction); // 벽을 만나면 우회할 방향 찾기
        }

        rb.linearVelocity = direction * moveSpeed;

        if (direction == Vector2.zero)
        {
            animator.SetBool("walk", false);
            animator.SetBool("idle", true);
        }
        else
        {
            animator.SetBool("walk", true);
            animator.SetBool("idle", false);
        }

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            AttackPlayer();
        }
    }

    private Vector2 FindAlternativeDirection(Vector2 originalDirection)
    {
        Vector2[] possibleDirections = {
        new Vector2(originalDirection.x, 0),  // 좌우 이동 우선
        new Vector2(0, originalDirection.y),  // 상하 이동
        new Vector2(-originalDirection.x, 0), // 반대 방향
        new Vector2(0, -originalDirection.y)  // 반대 방향 (상하)
    };

        foreach (Vector2 dir in possibleDirections)
        {
            if (!WallAhead(dir))
            {
                return dir; // 벽이 없는 방향으로 이동
            }
        }

        return Vector2.zero; // 모든 방향이 막혀있다면 멈춤
    }

    private void AttackPlayer()
    {
        if (attackTimer <= 0)
        {
            animator.SetTrigger("attack");
            attackTimer = attackCooldown;

            Debug.Log("몬스터가 공격을 실행함"); // 몬스터가 공격을 시도하는지 확인

            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, LayerMask.GetMask("Player"));

            foreach (var player in hitPlayers)
            {
                Debug.Log("플레이어를 감지함! 데미지 입힘");
                player.GetComponent<Player>()?.TakeDamage(1); // 공격력 만큼 데미지 입힘
            }

        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        animator.SetTrigger("die");
        rb.linearVelocity = Vector2.zero;
        Destroy(gameObject, 2f);
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
    }

    private bool WallAhead(Vector2 direction)
    {
        float checkDistance = 1.0f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, checkDistance, LayerMask.GetMask("Wall"));

        Debug.DrawRay(transform.position, direction * checkDistance, Color.red, 0.1f);

        return hit.collider != null;
    }
}
