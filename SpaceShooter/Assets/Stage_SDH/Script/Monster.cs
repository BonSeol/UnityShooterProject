using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class Monster : MonoBehaviour
{
    [SerializeField] protected Transform player; // �÷��̾� ��ġ
    [SerializeField] protected GameObject bulletPrefab; // �߻��� �Ѿ� ������
    [SerializeField] protected Transform firePoint; // �Ѿ� �߻� ����
    [SerializeField] protected FlashEffect flashEffect; // ���� �޾��� �� ȿ��
    [SerializeField] protected float bulletSpeed = 5f; // �Ѿ� �ӵ�
    [SerializeField] protected float followRange = 5f; // �÷��̾ ������ ����
    [SerializeField] protected float shootRange = 4f; // ���� ������ ����
    [SerializeField] protected float moveSpeed = 2f; // �̵� �ӵ�
    [SerializeField] protected float shootYOffset = 1.5f; // ���� Y ������
    [SerializeField] protected int maxHp = 100; // �ִ� ü��
    [SerializeField] protected float shootInterval = 0.5f; // ���� ����

    [SerializeField] protected AudioClip shootSound;
    protected AudioSource audioSource;

    public bool IsSlowed = false;
    protected int hp; // ���� ü��
    protected float lastShootTime = 0f; // ������ ���� �ð�
    protected bool isFacingRight = true; // ��������Ʈ�� �������� ���� �ִ��� ����
    protected bool isDead = false; // ���Ͱ� �׾����� ����
    protected Animator animator; // �ִϸ�����
    protected Rigidbody2D rb; // ���� ����
    protected SpriteRenderer spriteRenderer; // ��������Ʈ ������

    private float nextDirectionChangeTime = 0f; // ���� ���� �ð�
    private float randomMoveTime = 0f; // ���� �̵� ���� �ð�
    private bool isMoving = true; // �̵� ������ ����
    private bool isSpawning = true; // ���� ���� �� ����
    private Vector2 moveDirection; // ���� �̵� ����
    private Vector2 lastDirection; // ������ �̵� ����
    private Coroutine slowEffectCoroutine; // �ӵ� ���� ȿ�� �ڷ�ƾ


    protected virtual void Start()
    {
        // �ʿ��� ������Ʈ �ʱ�ȭ
        player = GameObject.FindWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashEffect = GetComponent<FlashEffect>();
        audioSource = GetComponent<AudioSource>();
        hp = maxHp;

        // ���� �ִϸ��̼� ����
        animator.Play("Spawn");
        rb.linearVelocity = Vector2.zero; // �̵� ����

        // ���� �ð� �� ���� ����
        StartCoroutine(EndSpawnAnimation());
    }

    private IEnumerator EndSpawnAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // �ִϸ��̼� ���� ������ ���
        isSpawning = false; // ���� �Ϸ�
    }

    protected virtual void Update()
    {
        if (isSpawning || player == null || hp <= 0 || isDead)
            return;

        float distance = Vector2.Distance(transform.position, player.position); // �÷��̾���� �Ÿ� ���

        // ���� ���� ���� ������ ����
        if (distance <= shootRange && Mathf.Abs(player.position.y - transform.position.y) <= shootYOffset)
        {
            Shoot();
        }
        // ���� ���� ���� ������ �÷��̾� ����
        else if (distance <= followRange)
        {
            FollowPlayer();
        }
        // ���� ���̸� �������� �̵�
        else
        {
            RandomMove();
        }
    }

    // �̵� �ӵ��� �������� �޼���
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    // �̵� �ӵ��� �����ϴ� �޼���
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    protected virtual void FollowPlayer()
    {
        if (isDead) return;

        animator.Play("Move"); // �̵� �ִϸ��̼�
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed; // �̵�
        FlipSprite(direction.x); // ���� ��ȯ
    }

    protected virtual void Shoot()
    {
        if (isDead) return;

        // ���� ������ �ΰ� ����
        if (Time.time - lastShootTime >= shootInterval)
        {
            animator.Play("Attack");
            rb.linearVelocity = Vector2.zero; // �̵� ����
            lastShootTime = Time.time;
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed; // ���� ����
        }
    }

    protected virtual void ShootBullet()
    {
        if (isDead) return;

        // �Ѿ� �߻�
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        Vector2 shootDirection = (player.position - firePoint.position).normalized;
        bulletRb.linearVelocity = shootDirection * bulletSpeed;
        audioSource.PlayOneShot(shootSound);

        // �Ѿ� ȸ��
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        FlipSprite(shootDirection.x); // ��������Ʈ ���� ����
    }

    // ���Ͱ� �������� �ʰ� ��� ������ ��
    protected virtual void Idle()
    {
        if (isDead) return;

        animator.Play("Idle");
        rb.linearVelocity = Vector2.zero; // �̵� ����
    }

    // �������� �̵�
    protected void RandomMove()
    {
        if (isDead) return;

        if (Time.time >= nextDirectionChangeTime)
        {
            if (Time.time >= randomMoveTime)
            {
                if (Random.value > 1f) // 0% Ȯ���� ������ ����
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
            rb.linearVelocity = moveDirection * moveSpeed; // �̵�

            // ���� �̵��� ���� ��ȯ
            FlipSprite(moveDirection.x);
        }
    }

    // ����Ʈ�� ���� ���� ����
    private void ChooseSmartRandomDirection()
    {
        Vector2 newDirection;
        int attempts = 0;
        do
        {
            // �������� x, y ���� �����Ͽ� �̵� ���� ����
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);

            newDirection = new Vector2(x, y).normalized; // ���ο� ���� ���

            attempts++;
        }
        while ((Vector2.Dot(newDirection, lastDirection) > 0.5f || newDirection.magnitude < 0.3f) && attempts < 5);

        lastDirection = moveDirection;
        moveDirection = newDirection;

        FlipSprite(moveDirection.x); // ���� ��ȯ
    }

    // ���� �浹 �� ���� ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            ChooseSmartRandomDirection(); // ���� �ε����� ���ο� ���� ����
        }
    }

    // ��������Ʈ ���� ��ȯ
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

    // ���ظ� �޾��� �� ó��
    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        hp -= damage;
        if (hp <= 0)
            Die(); // ü���� 0 ���Ϸ� �������� ���� ó��
        else
            flashEffect.Flash(); // ���ظ� ������ �÷��� ȿ��
    }

    // ���� ���� ó��
    protected virtual void Die()
    {
        isDead = true;
        GetComponent<Collider2D>().isTrigger = true; // �浹 ó�� ��Ȱ��ȭ
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // ���� ��� ����
        animator.Play("Die");

        // ���� �� ���� �ִϸ��̼� ����
        StartCoroutine(PlayExplodeAnimation());
    }

    // ���� �ִϸ��̼� ���� �� ���� ����
    private IEnumerator PlayExplodeAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // ���� �ִϸ��̼� ���� ������ ���
        yield return new WaitForSeconds(0.8f); // ��� ���

        animator.Play("Explode"); // ���� �ִϸ��̼�

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // ���� �ִϸ��̼� ���� ������ ���
        Destroy(gameObject); // ���� ��ü ����
    }

    // �ӵ� ���Ҹ� ó���ϴ� �޼���
    public void ApplySlowEffect(float slowAmount, float duration)
    {
        float originalSpeed = moveSpeed; // ������ ���� �ӵ� ����
        //Debug.Log(originalSpeed);

        // ���� �ӵ� ����
        moveSpeed = originalSpeed - slowAmount;

        // ���ο� ���� �÷��� ����
        IsSlowed = true;

        // ���� �ð� ���� ���
        StartCoroutine(ResetSpeedAfterTime(originalSpeed, duration));
    }

    private IEnumerator ResetSpeedAfterTime(float originalSpeed, float duration)
    {
        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed; // �ӵ� ����
        IsSlowed = false;  // ���ο� ����
    }
}
