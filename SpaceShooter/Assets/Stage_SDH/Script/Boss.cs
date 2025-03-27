using UnityEngine;
using System.Collections;

public class Boss : Monster
{
    [SerializeField] private AudioClip screamSound; // 총알 발사 시 사용할 오디오 클립
    private bool isChasing = false; // 플레이어를 추적 중인지 여부

    protected override void Start()
    {
        base.Start();
        audioSource.PlayOneShot(screamSound); // 발사 음원 재생
    }

    // 업데이트 함수: 매 프레임마다 호출
    protected override void Update()
    {
        // 죽었거나 플레이어가 없거나 HP가 0 이하일 경우 실행하지 않음
        if (isDead || player == null || hp <= 0)
            return;

        float distance = Vector2.Distance(transform.position, player.position); // 플레이어와의 거리 계산

        // 공격 범위에 도달하면 공격 모드로 전환
        if (distance <= shootRange && Mathf.Abs(player.position.y - transform.position.y) <= shootYOffset)
        {
            if (!isChasing) // 추적 중이 아니면 공격 시작
            {
                isChasing = false; // 추적 종료
                StopMoving(); // 이동 멈추기
            }
            Shoot(); // 공격 실행
        }
        // 플레이어가 추적 범위 내에 있을 때
        else if (distance <= followRange)
        {
            if (!isChasing) // 추적을 시작
            {
                isChasing = true;
            }
            FollowPlayer(); // 플레이어 추적
        }
        // 추적 범위 밖이면 랜덤으로 이동
        else
        {
            if (isChasing) // 추적 중이면 이동 멈추기
            {
                isChasing = false;
                StopMoving();
            }
            RandomMove(); // 랜덤 이동
        }
    }

    // 이동을 멈추는 함수
    private void StopMoving()
    {
        rb.linearVelocity = Vector2.zero; // 속도 초기화
        animator.Play("Idle"); // 애니메이션: 대기 상태
    }

    // 플레이어를 추적하는 함수
    protected override void FollowPlayer()
    {
        if (isDead) return;

        animator.Play("Move"); // 애니메이션: 이동 상태
        Vector2 direction = (player.position - transform.position).normalized; // 플레이어 방향 계산
        rb.linearVelocity = direction * moveSpeed; // 이동 속도 적용
        FlipSprite(direction.x); // 방향에 맞게 스프라이트 회전
    }

    // 공격 함수
    protected override void Shoot()
    {
        if (isDead) return;

        if (Time.time - lastShootTime >= shootInterval) // 공격 간격 체크
        {
            rb.linearVelocity = Vector2.zero; // 이동 멈추기
            lastShootTime = Time.time; // 마지막 공격 시간 업데이트

            // 랜덤으로 공격 패턴 선택
            ChooseShootPattern();
        }
    }

    // 공격 패턴을 랜덤으로 선택하는 함수
    private void ChooseShootPattern()
    {
        int pattern = Random.Range(0, 4); // 4가지 패턴 중 랜덤 선택
        switch (pattern)
        {
            case 0: BasicShoot(); break;  // 기본 발사
            case 1: TriShot(); break;     // 삼연발 발사
            case 2: CircleShoot(); break; // 360도 발사
            case 3: MultiShot(); break;   // 여러 발 차례대로 발사
        }
    }

    // 애니메이션이 끝날 때까지 기다리는 코루틴
    private IEnumerator WaitForShootAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // 애니메이션이 끝날 때까지 대기
        animator.Play("Idle"); // "Idle" 애니메이션으로 전환
    }

    // 총알 발사 함수
    private void ShootBullet(Vector2 direction)
    {
        if (isDead) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity); // 총알 생성
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = direction * bulletSpeed; // 총알 속도 적용

        // 총알의 회전 각도 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // 회전 적용

        FlipSprite(direction.x); // 발사 방향에 맞게 스프라이트 회전
    }

    // 기본 발사 함수 (플레이어 방향으로 발사)
    public void CallBasicShoot()
    {
        audioSource.PlayOneShot(shootSound); // 발사 음원 재생
        ShootBullet((player.position - firePoint.position).normalized); // 플레이어 방향으로 총알 발사
    }

    // 기본 발사 애니메이션 실행
    private void BasicShoot()
    {
        animator.Play("CallBasicShoot"); // 애니메이션: 공격
        StartCoroutine(WaitForShootAnimation()); // 애니메이션 완료 후 idle 상태로 전환
    }

    // 삼연발 발사 함수
    public void CallTriShot()
    {
        audioSource.PlayOneShot(shootSound); // 발사 음원 재생

        // 중앙, 20도 회전, -20도 회전 총 3발 발사
        Vector2 direction = (player.position - firePoint.position).normalized;
        ShootBullet(direction); // 중앙 발사
        ShootBullet(Quaternion.Euler(0, 0, 20) * direction); // 20도 회전한 방향으로 발사
        ShootBullet(Quaternion.Euler(0, 0, -20) * direction); // -20도 회전한 방향으로 발사
    }

    // 삼연발 애니메이션 실행
    private void TriShot()
    {
        animator.Play("CallTriShot"); // 애니메이션: 공격
        StartCoroutine(WaitForShootAnimation()); // 애니메이션 완료 후 idle 상태로 전환
    }

    // 360도 발사 함수 (16발 발사)
    private void CircleShoot()
    {
        float originalBulletSpeed = bulletSpeed; // 원래 bulletSpeed 저장
        bulletSpeed = 0.1f;  // 360도 공격 중에는 bulletSpeed를 0.1로 설정

        audioSource.PlayOneShot(screamSound); // 발사 음원 재생

        for (int j = 0; j < 3; j++) // 3번 반복
        {
            animator.Play("CallCircleShoot"); // 애니메이션: 패턴 공격
            WaitForShootAnimation(); // 애니메이션 완료 대기
        }

        bulletSpeed = originalBulletSpeed;  // 원래 bulletSpeed로 복구
        animator.Play("Idle"); // "Idle" 애니메이션으로 전환
    }

    // 360도 방향으로 총알 발사 (16발, 22.5도 간격)
    public void CallCircleShoot()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero; // 이동 멈추기

        // 360도 방향으로 총알 발사
        for (int i = 0; i < 16; i++)
        {
            float angle = i * (360f / 16f); // 360도를 16발로 나누기
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)); // 각도에 맞는 방향 계산
            ShootBullet(direction); // 총알 발사
        }
    }

    // 5발을 차례대로 발사하는 함수
    public void MultiShot()
    {
        StartCoroutine(MultiShotCoroutine()); // 코루틴을 이용한 5발 연속 발사
    }

    // 5발을 차례대로 발사하는 코루틴
    private IEnumerator MultiShotCoroutine()
    {
        animator.Play("CallMultiShot"); // 애니메이션: 공격
        // 5발을 순차적으로 발사
        for (int i = 0; i < 5; i++)
        {
            audioSource.PlayOneShot(shootSound); // 발사 음원 재생
            Vector2 direction = (player.position - firePoint.position).normalized; // 플레이어 방향 계산
            ShootBullet(direction); // 총알 발사
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // 애니메이션 대기
        }

        animator.Play("Idle"); // "Idle" 애니메이션으로 전환
    }

    // 스프라이트의 방향을 맞추는 함수
    protected override void FlipSprite(float directionX)
    {
        if (directionX > 0 && isFacingRight)  // 오른쪽 방향일 때
        {
            isFacingRight = false;
            spriteRenderer.flipX = true;
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
        else if (directionX < 0 && !isFacingRight)  // 왼쪽 방향일 때
        {
            isFacingRight = true;
            spriteRenderer.flipX = false;
            firePoint.localPosition = new Vector3(-Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
    }

    // 데미지를 받는 함수
    public override void TakeDamage(int damage)
    {
        if (isDead) return;

        hp -= damage; // HP 차감
        if (hp <= 0)
            Die(); // 사망 처리
        else
            flashEffect.Flash(); // 데미지 이펙트
    }
}