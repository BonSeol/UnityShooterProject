using System;
using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    public Animator animator;
    public Transform[] waypoints; // �̵��� ������
    public Transform player; // �÷��̾� ��ġ
    public float moveSpeed = 2f;  // �̵� �ӵ�
    public float idleTime = 2f;   // �����ִ� �ð�
    public int maxHP = 30;        // ���� ü��
    public float attackRange = 5f; // ���� ����
    private int currentHP;
    private int moveCount = 0; // �̵� Ƚ�� ī��Ʈ �߰�

    private int currentWaypointIndex = 0;
    private bool isDead = false;
    private bool isIdle = false; // idle ���� üũ
    private bool isAttacking = false; // ���� ������ üũ

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
            Debug.DrawLine(transform.position, player.position, Color.red); // �Ÿ� �ð�ȭ

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
        Debug.Log("attack2 ����");
    }

    public void StartAttack2()
    {
        isAttacking = true;
        Debug.Log("attack2 ����");
    }

    public void EndAttack()
    {
        isAttacking = false;
        Debug.Log("���� ����");
    }

    void CreateMissile()
    {
        if (isAttacking)
        {
            Instantiate(missile, pos1.position, Quaternion.identity);
            Instantiate(missile, pos2.position, Quaternion.identity);
            Instantiate(missile, pos3.position, Quaternion.identity);
            Instantiate(missile, pos4.position, Quaternion.identity);

            // ��� ȣ��
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
            Gizmos.DrawWireSphere(transform.position, attackRange); // ���� ���� ǥ��
        }
    }

    IEnumerator MoveLoop()
    {
        while (!isDead)
        {
            Transform target = waypoints[currentWaypointIndex];

            isAttacking = false; // �̵� ������ �� ���� ���� ����
            animator.Play("mage_walk");
            isIdle = false;

            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            moveCount++; // �̵� Ƚ�� ����

            if (moveCount == 3) // 3��° �̵� �� ����
            {
                animator.Play("mage_attack2");
                isAttacking = true;
                yield return new WaitForSeconds(2f); // ���� �ִϸ��̼� �ð� (�ʿ��ϸ� ����)
                moveCount = 0; // �ٽ� ī��Ʈ �ʱ�ȭ
            }
            else
            {
                animator.Play("mage_attack1");
                isAttacking = true;
                yield return new WaitForSeconds(4f); // attack1�� ���� �ӹ��� (�ð� ���� ����)
            }

            animator.Play("mage_idle");
            isIdle = true;
            yield return new WaitForSeconds(idleTime);

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // ������ �̹� �׾����� �������� ����

        currentHP -= damage;
        StopAllCoroutines(); // �̵� ���߱�
        animator.Play("mage_hit");
        isIdle = false;

        if (currentHP <= 0)
        {
            Die();
            return; // �׾����� �� �̻� �������� ����
        }

        StartCoroutine("ResumeAfterHit"); // 0.5�� ��ٷȴٰ� �̵� �簳
    }

    IEnumerator ResumeAfterHit()
    {
        yield return new WaitForSeconds(0.5f); // Hit �ִϸ��̼� ��� �ð�
        if (!isDead) StartCoroutine(MoveLoop());
    }


    void Die()
    {
        isDead = true;
        animator.Play("mage_death");
        StopAllCoroutines(); // �̵� ����
        CancelInvoke("CreateMissile"); // �̻��� ���� ����

        onDeath?.Invoke();  // �ܺο� �׾��ٰ� �˸�

        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet")) // �Ѿ˿� �¾��� ��
        {
            TakeDamage(10);
            Destroy(other.gameObject); // �Ѿ� ����
        }
    }
}