using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Boss_Golem : MonoBehaviour
{
    // 애니메이터 컴포넌트
    Animator animator;


    public float maxHealth = 100f;
    public float currentHealth;

    private bool isAttacking = false;
    private bool isCooldownActive = false;
    private bool enraged = false; // 체력 30% 이하 상태 체크

    private List<int> attackPatterns = new List<int> { 1, 2, 3 }; // 패턴 목록
    private Queue<int> attackQueue = new Queue<int>(); // 섞인 패턴을 저장할 큐

    public GameObject[] pattern1Object;  // 패턴1 던질 오브젝트 프리팹
    public float throwForce = 20f;  // 던지는 힘 (값을 적절하게 설정)
    public float throwForce_2 = 30f;  // 던지는 힘 (값을 적절하게 설정)

    public GameObject pattern2Object;  // 패턴2 던질 오브젝트 프리팹
    public Transform target;  // 타겟(플레이어)의 위치
    public float spawnInterval = 2f;  // 오브젝트 소환 주기
    public int numProjectiles = 3;  // 생성할 오브젝트 개수
    private List<GameObject> projectiles = new List<GameObject>();  // 생성된 오브젝트 리스트

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth; // 보스 체력 초기화
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
                int attackPattern = attackQueue.Dequeue(); // 큐에서 패턴 가져오기

                StartCoroutine(ExecuteAttack(attackPattern));

                if (attackQueue.Count == 0) // 모든 패턴을 사용했으면 다시 섞기
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

        float cooldownMultiplier = (currentHealth / maxHealth <= 0.3f) ? 0.5f : 1f; // 체력 30% 이하이면 쿨타임 절반 감소
        float cooldown = 1f; // 기본 쿨타임

        switch (pattern)
        {
            case 1:
                animator.SetTrigger("attack");
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(Pattern1()); // 6개 던지고 3초 후 빈 공간 던짐
                cooldown = 5f;
                break;
            case 2:
                animator.SetTrigger("magic");
                yield return new WaitForSeconds(1.2f);
                StartCoroutine(Pattern2()); // 2초 간격으로 생성되는 오브젝트 2개 를 3초딜레이 주면서 던짐
                cooldown = 10f;
                break;
        }

        animator.SetTrigger("idle"); // 공격 후 Idle 상태

        isAttacking = false;

        if (currentHealth / maxHealth <= 0.3f && !enraged)
        {
            enraged = true;
            cooldown = 0; // 즉시 쿨타임 초기화
        }

        if (cooldown > 0) // 0초일 경우 기다릴 필요 없음
        {
            isCooldownActive = true;
            yield return new WaitForSeconds(cooldown * cooldownMultiplier); // 체력에 따른 쿨타임 적용
            isCooldownActive = false;
        }
    }

    // 보스 체력 감소 함수 (외부에서 호출 가능)
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth / maxHealth <= 0.3f && !enraged)
        {
            enraged = true;
            StopAllCoroutines();
            StartCoroutine(AttackRoutine()); // 즉시 패턴 다시 실행
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
        // 추가적으로 사망 로직을 넣을 수 있음
    }


    private IEnumerator Pattern1()
    {
        ThrowObjectsInCircle();  // 첫 번째 투척
        yield return new WaitForSeconds(3f); // 3초 대기
        ThrowObjectsInCircleOffset();  // 두 번째 투척 (빈 공간에)
    }

    // 원형으로 6개의 오브젝트 던지는 함수
    private void ThrowObjectsInCircle()
    {
        float angleStep = 360f / pattern1Object.Length; // 각 오브젝트 간 각도 간격
        float startAngle = 0f; // 시작 각도

        for (int i = 0; i < pattern1Object.Length; i++)
        {
            float angle = startAngle + (angleStep * i);
            ThrowProjectileAtAngle(angle, i); // i번째 오브젝트를 던짐
        }
    }

    // 원형으로 6개의 오브젝트를 던지되, 기존과 오프셋을 줘서 빈 공간에 던지는 함수
    private void ThrowObjectsInCircleOffset()
    {
        float angleStep = 360f / pattern1Object.Length;
        float startAngle = angleStep / 2; // 기존 투척 위치와 어긋나게 설정

        for (int i = 0; i < pattern1Object.Length; i++)
        {
            float angle = startAngle + (angleStep * i);
            ThrowProjectileAtAngle(angle, i); // i번째 오브젝트를 던짐
        }
    }

    // 특정 각도로 오브젝트를 던지는 함수
    private void ThrowProjectileAtAngle(float angle, int index)
    {
        float radian = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        Vector2 throwDirection = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;

        // 투사체 생성 위치 (보스의 위치에서 조금 떨어진 곳)
        Vector3 spawnPosition = transform.position + (Vector3)(throwDirection * 1.5f);

        // 해당 오브젝트 생성 (각각 다른 오브젝트를 사용)
        GameObject projectile = Instantiate(pattern1Object[index], spawnPosition, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }

        BossDamage bossdamage = projectile.AddComponent<BossDamage>();
        bossdamage.damage = 1.0f;  // 피해량 설정 (필요 시 조정)

        Destroy(projectile, 2f);
    }




    private IEnumerator Pattern2()
    {
        // 오브젝트 소환
        for (int i = 0; i < numProjectiles; i++)
        {
            // 오브젝트를 소환할 위치 계산
            Vector3 spawnPosition = transform.position + new Vector3(0, i * 2f, 0);  // 보스 주변의 약간 떨어진 위치로 설정

            // 오브젝트 생성
            GameObject projectile = Instantiate(pattern2Object, spawnPosition, Quaternion.identity);
            projectiles.Add(projectile);  // 생성된 오브젝트를 리스트에 추가


            BossDamage bossdamage = projectile.AddComponent<BossDamage>();
            bossdamage.damage = 1.5f;  // 피해량 설정 (필요 시 조정)

            // 2초 간격으로 오브젝트를 생성
            yield return new WaitForSeconds(spawnInterval);
        }

        // 각 오브젝트를 던지는 로직
        foreach (GameObject projectile in projectiles)
        {
            // 던질 방향 계산
            Vector3 targetPosition = target.position;
            Vector3 spawnPosition = projectile.transform.position;
            Vector2 throwDirection = (targetPosition - spawnPosition).normalized; // 타겟 방향으로 계산

            // Rigidbody2D를 가져와서
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 던질 방향과 힘을 적용
                rb.AddForce(throwDirection * throwForce_2, ForceMode2D.Impulse);
            }

            // 던지기 전에 3초 대기
            yield return new WaitForSeconds(3f);

            Destroy(projectile, 5f);
        }

    }


}
