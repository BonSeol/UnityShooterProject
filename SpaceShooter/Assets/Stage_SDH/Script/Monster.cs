using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class Monster : MonoBehaviour
{
    [SerializeField] protected Transform player; // 플레이어 위치
    [SerializeField] protected GameObject bulletPrefab; // 발사할 총알 프리팹
    [SerializeField] protected Transform firePoint; // 총알 발사 지점
    [SerializeField] protected FlashEffect flashEffect; // 피해 받았을 때 효과
    [SerializeField] protected float bulletSpeed = 5f; // 총알 속도
    [SerializeField] protected float followRange = 5f; // 플레이어를 추적할 범위
    [SerializeField] protected float shootRange = 4f; // 공격 가능한 범위
    [SerializeField] protected float moveSpeed = 2f; // 이동 속도
    [SerializeField] protected float shootYOffset = 1.5f; // 공격 Y 오프셋
    [SerializeField] protected int maxHp = 100; // 최대 체력
    [SerializeField] protected float shootInterval = 0.5f; // 공격 간격

    [SerializeField] protected AudioClip shootSound;
    protected AudioSource audioSource;

    public bool IsSlowed = false;
    protected int hp; // 현재 체력
    protected float lastShootTime = 0f; // 마지막 공격 시간
    protected bool isFacingRight = true; // 스프라이트가 오른쪽을 보고 있는지 여부
    protected bool isDead = false; // 몬스터가 죽었는지 여부
    protected Animator animator; // 애니메이터
    protected Rigidbody2D rb; // 물리 엔진
    protected SpriteRenderer spriteRenderer; // 스프라이트 렌더러

    private float nextDirectionChangeTime = 0f; // 방향 변경 시간
    private float randomMoveTime = 0f; // 랜덤 이동 지속 시간
    private bool isMoving = true; // 이동 중인지 여부
    private bool isSpawning = true; // 몬스터 생성 중 여부
    private Vector2 moveDirection; // 현재 이동 방향
    private Vector2 lastDirection; // 마지막 이동 방향
    private Coroutine slowEffectCoroutine; // 속도 감소 효과 코루틴


    protected virtual void Start()
    {
        // 필요한 컴포넌트 초기화
        player = GameObject.FindWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashEffect = GetComponent<FlashEffect>();
        audioSource = GetComponent<AudioSource>();
        hp = maxHp;

        // 생성 애니메이션 실행
        animator.Play("Spawn");
        rb.linearVelocity = Vector2.zero; // 이동 멈춤

        // 일정 시간 후 동작 시작
        StartCoroutine(EndSpawnAnimation());
    }

    private IEnumerator EndSpawnAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // 애니메이션 끝날 때까지 대기
        isSpawning = false; // 생성 완료
    }

    protected virtual void Update()
    {
        if (isSpawning || player == null || hp <= 0 || isDead)
            return;

        float distance = Vector2.Distance(transform.position, player.position); // 플레이어와의 거리 계산

        // 공격 범위 내에 있으면 공격
        if (distance <= shootRange && Mathf.Abs(player.position.y - transform.position.y) <= shootYOffset)
        {
            Shoot();
        }
        // 추적 범위 내에 있으면 플레이어 추적
        else if (distance <= followRange)
        {
            FollowPlayer();
        }
        // 범위 밖이면 랜덤으로 이동
        else
        {
            RandomMove();
        }
    }

    // 이동 속도를 가져오는 메서드
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    // 이동 속도를 설정하는 메서드
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    protected virtual void FollowPlayer()
    {
        if (isDead) return;

        animator.Play("Move"); // 이동 애니메이션
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed; // 이동
        FlipSprite(direction.x); // 방향 전환
    }

    protected virtual void Shoot()
    {
        if (isDead) return;

        // 공격 간격을 두고 공격
        if (Time.time - lastShootTime >= shootInterval)
        {
            animator.Play("Attack");
            rb.linearVelocity = Vector2.zero; // 이동 멈춤
            lastShootTime = Time.time;
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed; // 방향 설정
        }
    }

    protected virtual void ShootBullet()
    {
        if (isDead) return;

        // 총알 발사
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        Vector2 shootDirection = (player.position - firePoint.position).normalized;
        bulletRb.linearVelocity = shootDirection * bulletSpeed;
        audioSource.PlayOneShot(shootSound);

        // 총알 회전
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        FlipSprite(shootDirection.x); // 스프라이트 방향 설정
    }

    // 몬스터가 움직이지 않고 대기 상태일 때
    protected virtual void Idle()
    {
        if (isDead) return;

        animator.Play("Idle");
        rb.linearVelocity = Vector2.zero; // 이동 멈춤
    }

    // 랜덤으로 이동
    protected void RandomMove()
    {
        if (isDead) return;

        if (Time.time >= nextDirectionChangeTime)
        {
            if (Time.time >= randomMoveTime)
            {
                if (Random.value > 1f) // 0% 확률로 가만히 있음
                {
                    isMoving = false;
                    rb.linearVelocity = Vector2.zero;
                    animator.Play("Idle");
                }
                else
                {
                    ChooseSmartRandomDirection();
                    isMoving = true;
                }
                nextDirectionChangeTime = Time.time + Random.Range(4f, 6f);
                randomMoveTime = Time.time + Random.Range(3f, 5f);
            }
        }

        if (isMoving)
        {
            animator.Play("Move");
            rb.linearVelocity = moveDirection * moveSpeed; // 이동

            // 랜덤 이동시 방향 전환
            FlipSprite(moveDirection.x);
        }
    }

    // 스마트한 랜덤 방향 선택
    private void ChooseSmartRandomDirection()
    {
        Vector2 newDirection;
        int attempts = 0;
        do
        {
            // 랜덤으로 x, y 값을 설정하여 이동 방향 결정
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);

            newDirection = new Vector2(x, y).normalized; // 새로운 방향 계산

            attempts++;
        }
        while ((Vector2.Dot(newDirection, lastDirection) > 0.5f || newDirection.magnitude < 0.3f) && attempts < 5);

        lastDirection = moveDirection;
        moveDirection = newDirection;

        FlipSprite(moveDirection.x); // 방향 전환
    }

    // 벽과 충돌 시 방향 변경
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            ChooseSmartRandomDirection(); // 벽에 부딪히면 새로운 방향 설정
        }
    }

    // 스프라이트 방향 전환
    protected virtual void FlipSprite(float directionX)
    {
        if (directionX > 0 && !isFacingRight)
        {
            isFacingRight = true;
            spriteRenderer.flipX = false;
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
        else if (directionX < 0 && isFacingRight)
        {
            isFacingRight = false;
            spriteRenderer.flipX = true;
            firePoint.localPosition = new Vector3(-Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
    }

    // 피해를 받았을 때 처리
    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        hp -= damage;
        if (hp <= 0)
            Die(); // 체력이 0 이하로 떨어지면 죽음 처리
        else
            flashEffect.Flash(); // 피해를 받으면 플래시 효과
    }

    // 몬스터 죽음 처리
    protected virtual void Die()
    {
        isDead = true;
        GetComponent<Collider2D>().isTrigger = true; // 충돌 처리 비활성화
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // 물리 계산 멈춤
        animator.Play("Die");

        // 죽은 후 폭발 애니메이션 실행
        StartCoroutine(PlayExplodeAnimation());
    }

    // 폭발 애니메이션 실행 후 몬스터 삭제
    private IEnumerator PlayExplodeAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // 죽음 애니메이션 끝날 때까지 대기
        yield return new WaitForSeconds(0.8f); // 잠시 대기

        animator.Play("Explode"); // 폭발 애니메이션

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // 폭발 애니메이션 끝날 때까지 대기
        Destroy(gameObject); // 몬스터 객체 삭제
    }

    // 속도 감소를 처리하는 메서드
    public void ApplySlowEffect(float slowAmount, float duration)
    {
        float originalSpeed = moveSpeed; // 몬스터의 원래 속도 저장
        //Debug.Log(originalSpeed);

        // 몬스터 속도 감소
        moveSpeed = originalSpeed - slowAmount;

        // 슬로우 상태 플래그 설정
        IsSlowed = true;

        // 일정 시간 동안 대기
        StartCoroutine(ResetSpeedAfterTime(originalSpeed, duration));
    }

    private IEnumerator ResetSpeedAfterTime(float originalSpeed, float duration)
    {
        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed; // 속도 복원
        IsSlowed = false;  // 슬로우 해제
    }
}
