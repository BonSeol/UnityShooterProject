using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Boss : Monster
{
    [SerializeField] private AudioClip screamSound; // 보스 등장 또는 특정 패턴에서 재생할 소리
    [SerializeField] private GameObject bulletPrefab2; // 총알 프리팹
    [SerializeField] private GameObject bulletPrefab3; // 총알 프리팹

    private bool isChasing = false; // 플레이어를 추적 중인지 여부

    protected override void Start()
    {
        base.Start();
        audioSource.PlayOneShot(screamSound); // 등장 시 효과음 재생
    }

    protected override void Update()
    {
        // 보스가 사망했거나, 플레이어가 없거나, HP가 0 이하라면 동작하지 않음
        if (isDead || player == null || hp <= 0)
            return;

        float distance = Vector2.Distance(transform.position, player.position); // 플레이어와의 거리 계산

        // 공격 범위 내에 있을 경우 공격 실행
        if (distance <= shootRange && Mathf.Abs(player.position.y - transform.position.y) <= shootYOffset)
        {
            if (!isChasing)
            {
                isChasing = false; // 추적 중지
                StopMoving(); // 이동 멈춤
            }
            Shoot(); // 공격 실행
        }
        // 플레이어가 추적 범위 내에 있을 경우
        else if (distance <= followRange)
        {
            if (!isChasing)
                isChasing = true;

            FollowPlayer(); // 플레이어 추적
        }
        // 추적 범위를 벗어난 경우
        else
        {
            if (isChasing)
            {
                isChasing = false;
                StopMoving();
            }
            RandomMove(); // 랜덤 이동
        }
    }

    // 보스 이동 정지
    private void StopMoving()
    {
        rb.linearVelocity = Vector2.zero; // 속도 초기화
        //animator.Play("Idle"); // 대기 애니메이션 실행
    }

    // 플레이어 추적
    protected override void FollowPlayer()
    {
        if (isDead) return;

        animator.Play("Move"); // 이동 애니메이션 실행
        Vector2 direction = (player.position - transform.position).normalized; // 이동 방향 계산
        rb.linearVelocity = direction * moveSpeed; // 이동 적용
        FlipSprite(direction.x); // 방향 전환
    }

    // 공격 실행
    protected override void Shoot()
    {
        if (isDead) return;

        if (Time.time - lastShootTime >= shootInterval) // 공격 가능 여부 체크
        {
            rb.linearVelocity = Vector2.zero; // 이동 멈춤
            lastShootTime = Time.time; // 마지막 공격 시간 갱신

            // 랜덤 패턴 선택
            ChooseShootPattern();
        }
    }

    // 랜덤 공격 패턴 선택
    private void ChooseShootPattern()
    {
        int pattern = Random.Range(0, 4); // 4가지 패턴 중 하나 선택
        switch (pattern)
        {
            case 0: BasicShoot(); break;
            case 1: TriShot(); break;
            case 2: CircleShoot(); break;
            case 3: MultiShot(); break;
        }
    }

    // 총알 발사 함수
    private void ShootBullet(Vector2 direction, GameObject bulletprefab)
    {
        if (isDead) return;

        GameObject bullet = Instantiate(bulletprefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = direction * bulletSpeed; // 총알 속도 적용

        // 총알 방향에 맞게 회전 적용
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        FlipSprite(direction.x);
    }

    // 애니메이션이 끝날 때까지 대기하는 코루틴
    private IEnumerator WaitForShootAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    // 기본 발사 (플레이어 방향으로 1발)
    public void CallBasicShoot()
    {
        audioSource.PlayOneShot(shootSound);
        ShootBullet((player.position - firePoint.position).normalized, bulletPrefab2);
    }

    private void BasicShoot()
    {
        animator.Play("CallBasicShoot");
        StartCoroutine(WaitForShootAnimation());
    }

    // 삼연발 공격 (중앙 + 20도, -20도)
    public void CallTriShot()
    {
        audioSource.PlayOneShot(shootSound);

        Vector2 direction = (player.position - firePoint.position).normalized;
        ShootBullet(direction, bulletPrefab2);
        ShootBullet(Quaternion.Euler(0, 0, 20) * direction, bulletPrefab2);
        ShootBullet(Quaternion.Euler(0, 0, -20) * direction, bulletPrefab2);
    }

    private void TriShot()
    {
        animator.Play("CallTriShot");
        StartCoroutine(WaitForShootAnimation());
    }

    // 360도 총알 발사 (16방향)
    public void CallCircleShoot()
    {
        rb.linearVelocity = Vector2.zero; // 이동 멈춤

        for (int i = 0; i < 16; i++)
        {
            float angle = i * (360f / 16f);
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
            ShootBullet(direction, bulletPrefab3);
        }
    }

    private void CircleShoot()
    {
        audioSource.PlayOneShot(screamSound);

        float originalBulletSpeed = bulletSpeed;
        bulletSpeed = 0.1f;

        for (int j = 0; j < 3; j++)
        {
            animator.Play("CallCircleShoot");
            WaitForShootAnimation();
        }

        bulletSpeed = originalBulletSpeed;
    }

    // 연속 사격 (5발)
    public void MultiShot()
    {
        StartCoroutine(MultiShotCoroutine());
    }

    private IEnumerator MultiShotCoroutine()
    {
        animator.Play("CallMultiShot");
        audioSource.PlayOneShot(shootSound);

        for (int i = 0; i < 5; i++)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;
            ShootBullet(direction, bulletPrefab);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    // 스프라이트 방향 전환
    protected override void FlipSprite(float directionX)
    {
        if (directionX > 0 && isFacingRight)
        {
            isFacingRight = false;
            spriteRenderer.flipX = true;
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
        else if (directionX < 0 && !isFacingRight)
        {
            isFacingRight = true;
            spriteRenderer.flipX = false;
            firePoint.localPosition = new Vector3(-Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
    }

    // 데미지 처리
    public override void TakeDamage(int damage)
    {
        if (isDead) return;

        hp -= damage;
        if (hp <= 0)
            Die();
        else
            flashEffect.Flash();
    }

    protected override void Die()
    {
        if (isDead) return; // 이미 죽었으면 실행하지 않음

        isDead = true;
        GetComponent<Collider2D>().enabled = false; // 충돌 비활성화
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // 물리 효과 제거

        animator.Play("Die");

        SceneManager.LoadScene("Scene_ChoiWonje"); // 씬 전환
    }

    public void CallExplode()
    {
        StartCoroutine(PlayExplodeAnimation());
    }

    private IEnumerator PlayExplodeAnimation()
    {
        animator.Play("Explode"); // 폭발 애니메이션 실행
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // 애니메이션 끝까지 대기
        Destroy(gameObject); // 보스 삭제
    }
}