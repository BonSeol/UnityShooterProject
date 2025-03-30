using System;
using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    public Animator animator;
    public Transform[] waypoints; // 이동할 지점들
    public Transform player; // 플레이어 위치
    public float moveSpeed = 2f;  // 이동 속도
    public float idleTime = 2f;   // 멈춰있는 시간
    public int maxHP = 30;        // 보스 체력
    public float attackRange = 5f; // 공격 범위
    private int currentHP;
    private int moveCount = 0; // 이동 횟수 카운트 추가

    private int currentWaypointIndex = 0;
    private bool isDead = false;
    private bool isIdle = false; // idle 상태 체크
    private bool isAttacking = false; // 공격 중인지 체크

    public System.Action onDeath;

    [Header("Missile Settings")]
    public float Delay = 1f;
    public GameObject missile;
    public GameObject hole1;
    public GameObject hole2;
    public GameObject hole3;
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;
    public Transform holePos1;
    public Transform holePos2;
    public Transform holePos3;

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        currentHP = maxHP;
        StartCoroutine(MoveLoop());
    }

    private void Update()
    {
        if (!isDead && player != null && isIdle && !isAttacking)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            Debug.DrawLine(transform.position, player.position, Color.red); // 거리 시각화

            if (distance > attackRange)
            {
                isAttacking = true;
                animator.Play("mage_attack1");
            }
            else
            {
                isAttacking = true;
                animator.Play("mage_attack2");
            }
        }


    }

    public void StartAttack1()
    {
        isAttacking = true;
        Debug.Log("attack2 시작");
    }

    public void StartAttack2()
    {
        isAttacking = true;
        Debug.Log("attack2 시작");
    }

    public void EndAttack()
    {
        isAttacking = false;
        Debug.Log("공격 종료");
    }

    void CreateMissile()
    {
        if (isAttacking)
        {
            Instantiate(missile, pos1.position, Quaternion.identity);
            Instantiate(missile, pos2.position, Quaternion.identity);
            Instantiate(missile, pos3.position, Quaternion.identity);
            Instantiate(missile, pos4.position, Quaternion.identity);

            // 재귀 호출
            Invoke("CreateMissile", Delay);
        }

    }

    public void SetHolePositions(Transform p1, Transform p2, Transform p3)
    {
        holePos1 = p1;
        holePos2 = p2;
        holePos3 = p3;
    }

    void CreateHole()
    {
        if (!isAttacking) return;
        
            Instantiate(hole1, holePos1.position, Quaternion.identity);
            Instantiate(hole2, holePos2.position, Quaternion.identity);
            Instantiate(hole3, holePos3.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange); // 공격 범위 표시
        }
    }

    IEnumerator MoveLoop()
    {
        while (!isDead)
        {
            Transform target = waypoints[currentWaypointIndex];

            isAttacking = false; // 이동 시작할 때 공격 상태 해제
            animator.Play("mage_walk");
            isIdle = false;

            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            moveCount++; // 이동 횟수 증가

            if (moveCount == 3) // 3번째 이동 후 공격
            {
                animator.Play("mage_attack2");
                isAttacking = true;
                yield return new WaitForSeconds(2f); // 공격 애니메이션 시간 (필요하면 조절)
                moveCount = 0; // 다시 카운트 초기화
            }
            else
            {
                animator.Play("mage_attack1");
                isAttacking = true;
                yield return new WaitForSeconds(4f); // attack1은 오래 머무름 (시간 조절 가능)
            }

            animator.Play("mage_idle");
            isIdle = true;
            yield return new WaitForSeconds(idleTime);

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // 보스가 이미 죽었으면 실행하지 않음

        currentHP -= damage;
        StopAllCoroutines(); // 이동 멈추기
        animator.Play("mage_hit");
        isIdle = false;

        if (currentHP <= 0)
        {
            Die();
            return; // 죽었으면 더 이상 진행하지 않음
        }

        StartCoroutine("ResumeAfterHit"); // 0.5초 기다렸다가 이동 재개
    }

    IEnumerator ResumeAfterHit()
    {
        yield return new WaitForSeconds(0.5f); // Hit 애니메이션 대기 시간
        if (!isDead) StartCoroutine(MoveLoop());
    }


    void Die()
    {
        isDead = true;
        animator.Play("mage_death");
        StopAllCoroutines(); // 이동 중지
        CancelInvoke("CreateMissile"); // 미사일 생성 중지

        onDeath?.Invoke();  // 외부에 죽었다고 알림

        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet")) // 총알에 맞았을 때
        {
            TakeDamage(10);
            Destroy(other.gameObject); // 총알 삭제
        }
    }
}