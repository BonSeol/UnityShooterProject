using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Boss_Golem : MonoBehaviour
{
    // �ִϸ����� ������Ʈ
    Animator animator;


    public float maxHealth = 100f;
    public float currentHealth;

    private bool isAttacking = false;
    private bool isCooldownActive = false;
    private bool enraged = false; // ü�� 30% ���� ���� üũ

    private List<int> attackPatterns = new List<int> { 1, 2, 3 }; // ���� ���
    private Queue<int> attackQueue = new Queue<int>(); // ���� ������ ������ ť

    public GameObject[] pattern1Object;  // ����1 ���� ������Ʈ ������
    public float throwForce = 20f;  // ������ �� (���� �����ϰ� ����)
    public float throwForce_2 = 30f;  // ������ �� (���� �����ϰ� ����)

    public GameObject pattern2Object;  // ����2 ���� ������Ʈ ������
    public Transform target;  // Ÿ��(�÷��̾�)�� ��ġ
    public float spawnInterval = 2f;  // ������Ʈ ��ȯ �ֱ�
    public int numProjectiles = 3;  // ������ ������Ʈ ����
    private List<GameObject> projectiles = new List<GameObject>();  // ������ ������Ʈ ����Ʈ

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth; // ���� ü�� �ʱ�ȭ
        ShufflePatterns();
        StartCoroutine(AttackRoutine());
    }

    private void ShufflePatterns()
    {
        List<int> shuffledList = new List<int>(attackPatterns);
        for (int i = 0; i < shuffledList.Count; i++)
        {
            int temp = shuffledList[i];
            int randomIndex = Random.Range(i, shuffledList.Count);
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }

        attackQueue.Clear();
        foreach (int pattern in shuffledList)
        {
            attackQueue.Enqueue(pattern);
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (!isAttacking && !isCooldownActive && attackQueue.Count > 0)
            {
                int attackPattern = attackQueue.Dequeue(); // ť���� ���� ��������

                StartCoroutine(ExecuteAttack(attackPattern));

                if (attackQueue.Count == 0) // ��� ������ ��������� �ٽ� ����
                {
                    ShufflePatterns();
                }
            }

            yield return null;
        }
    }

    private IEnumerator ExecuteAttack(int pattern)
    {
        isAttacking = true;

        float cooldownMultiplier = (currentHealth / maxHealth <= 0.3f) ? 0.5f : 1f; // ü�� 30% �����̸� ��Ÿ�� ���� ����
        float cooldown = 1f; // �⺻ ��Ÿ��

        switch (pattern)
        {
            case 1:
                animator.SetTrigger("attack");
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(Pattern1()); // 6�� ������ 3�� �� �� ���� ����
                cooldown = 5f;
                break;
            case 2:
                animator.SetTrigger("magic");
                yield return new WaitForSeconds(1.2f);
                StartCoroutine(Pattern2()); // 2�� �������� �����Ǵ� ������Ʈ 2�� �� 3�ʵ����� �ָ鼭 ����
                cooldown = 10f;
                break;
        }

        animator.SetTrigger("idle"); // ���� �� Idle ����

        isAttacking = false;

        if (currentHealth / maxHealth <= 0.3f && !enraged)
        {
            enraged = true;
            cooldown = 0; // ��� ��Ÿ�� �ʱ�ȭ
        }

        if (cooldown > 0) // 0���� ��� ��ٸ� �ʿ� ����
        {
            isCooldownActive = true;
            yield return new WaitForSeconds(cooldown * cooldownMultiplier); // ü�¿� ���� ��Ÿ�� ����
            isCooldownActive = false;
        }
    }

    // ���� ü�� ���� �Լ� (�ܺο��� ȣ�� ����)
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth / maxHealth <= 0.3f && !enraged)
        {
            enraged = true;
            StopAllCoroutines();
            StartCoroutine(AttackRoutine()); // ��� ���� �ٽ� ����
        }

        if (currentHealth <= 0)
        {
            Die();
            Destroy(gameObject, 3.0f);
        }
    }

    private void Die()
    {
        StopAllCoroutines();
        animator.SetTrigger("die");
        // �߰������� ��� ������ ���� �� ����
    }


    private IEnumerator Pattern1()
    {
        ThrowObjectsInCircle();  // ù ��° ��ô
        yield return new WaitForSeconds(3f); // 3�� ���
        ThrowObjectsInCircleOffset();  // �� ��° ��ô (�� ������)
    }

    // �������� 6���� ������Ʈ ������ �Լ�
    private void ThrowObjectsInCircle()
    {
        float angleStep = 360f / pattern1Object.Length; // �� ������Ʈ �� ���� ����
        float startAngle = 0f; // ���� ����

        for (int i = 0; i < pattern1Object.Length; i++)
        {
            float angle = startAngle + (angleStep * i);
            ThrowProjectileAtAngle(angle, i); // i��° ������Ʈ�� ����
        }
    }

    // �������� 6���� ������Ʈ�� ������, ������ �������� �༭ �� ������ ������ �Լ�
    private void ThrowObjectsInCircleOffset()
    {
        float angleStep = 360f / pattern1Object.Length;
        float startAngle = angleStep / 2; // ���� ��ô ��ġ�� ��߳��� ����

        for (int i = 0; i < pattern1Object.Length; i++)
        {
            float angle = startAngle + (angleStep * i);
            ThrowProjectileAtAngle(angle, i); // i��° ������Ʈ�� ����
        }
    }

    // Ư�� ������ ������Ʈ�� ������ �Լ�
    private void ThrowProjectileAtAngle(float angle, int index)
    {
        float radian = angle * Mathf.Deg2Rad; // ������ �������� ��ȯ
        Vector2 throwDirection = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;

        // ����ü ���� ��ġ (������ ��ġ���� ���� ������ ��)
        Vector3 spawnPosition = transform.position + (Vector3)(throwDirection * 1.5f);

        // �ش� ������Ʈ ���� (���� �ٸ� ������Ʈ�� ���)
        GameObject projectile = Instantiate(pattern1Object[index], spawnPosition, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }

        BossDamage bossdamage = projectile.AddComponent<BossDamage>();
        bossdamage.damage = 1.0f;  // ���ط� ���� (�ʿ� �� ����)

        Destroy(projectile, 2f);
    }




    private IEnumerator Pattern2()
    {
        // ������Ʈ ��ȯ
        for (int i = 0; i < numProjectiles; i++)
        {
            // ������Ʈ�� ��ȯ�� ��ġ ���
            Vector3 spawnPosition = transform.position + new Vector3(0, i * 2f, 0);  // ���� �ֺ��� �ణ ������ ��ġ�� ����

            // ������Ʈ ����
            GameObject projectile = Instantiate(pattern2Object, spawnPosition, Quaternion.identity);
            projectiles.Add(projectile);  // ������ ������Ʈ�� ����Ʈ�� �߰�


            BossDamage bossdamage = projectile.AddComponent<BossDamage>();
            bossdamage.damage = 1.5f;  // ���ط� ���� (�ʿ� �� ����)

            // 2�� �������� ������Ʈ�� ����
            yield return new WaitForSeconds(spawnInterval);
        }

        // �� ������Ʈ�� ������ ����
        foreach (GameObject projectile in projectiles)
        {
            // ���� ���� ���
            Vector3 targetPosition = target.position;
            Vector3 spawnPosition = projectile.transform.position;
            Vector2 throwDirection = (targetPosition - spawnPosition).normalized; // Ÿ�� �������� ���

            // Rigidbody2D�� �����ͼ�
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // ���� ����� ���� ����
                rb.AddForce(throwDirection * throwForce_2, ForceMode2D.Impulse);
            }

            // ������ ���� 3�� ���
            yield return new WaitForSeconds(3f);

            Destroy(projectile, 5f);
        }

    }


}
