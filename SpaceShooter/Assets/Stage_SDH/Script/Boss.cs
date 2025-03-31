using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Boss : Monster
{
    [SerializeField] private AudioClip screamSound; // ���� ���� �Ǵ� Ư�� ���Ͽ��� ����� �Ҹ�
    [SerializeField] private GameObject bulletPrefab2; // �Ѿ� ������
    [SerializeField] private GameObject bulletPrefab3; // �Ѿ� ������

    private bool isChasing = false; // �÷��̾ ���� ������ ����

    protected override void Start()
    {
        base.Start();
        audioSource.PlayOneShot(screamSound); // ���� �� ȿ���� ���
    }

    protected override void Update()
    {
        // ������ ����߰ų�, �÷��̾ ���ų�, HP�� 0 ���϶�� �������� ����
        if (isDead || player == null || hp <= 0)
            return;

        float distance = Vector2.Distance(transform.position, player.position); // �÷��̾���� �Ÿ� ���

        // ���� ���� ���� ���� ��� ���� ����
        if (distance <= shootRange && Mathf.Abs(player.position.y - transform.position.y) <= shootYOffset)
        {
            if (!isChasing)
            {
                isChasing = false; // ���� ����
                StopMoving(); // �̵� ����
            }
            Shoot(); // ���� ����
        }
        // �÷��̾ ���� ���� ���� ���� ���
        else if (distance <= followRange)
        {
            if (!isChasing)
                isChasing = true;

            FollowPlayer(); // �÷��̾� ����
        }
        // ���� ������ ��� ���
        else
        {
            if (isChasing)
            {
                isChasing = false;
                StopMoving();
            }
            RandomMove(); // ���� �̵�
        }
    }

    // ���� �̵� ����
    private void StopMoving()
    {
        rb.linearVelocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        //animator.Play("Idle"); // ��� �ִϸ��̼� ����
    }

    // �÷��̾� ����
    protected override void FollowPlayer()
    {
        if (isDead) return;

        animator.Play("Move"); // �̵� �ִϸ��̼� ����
        Vector2 direction = (player.position - transform.position).normalized; // �̵� ���� ���
        rb.linearVelocity = direction * moveSpeed; // �̵� ����
        FlipSprite(direction.x); // ���� ��ȯ
    }

    // ���� ����
    protected override void Shoot()
    {
        if (isDead) return;

        if (Time.time - lastShootTime >= shootInterval) // ���� ���� ���� üũ
        {
            rb.linearVelocity = Vector2.zero; // �̵� ����
            lastShootTime = Time.time; // ������ ���� �ð� ����

            // ���� ���� ����
            ChooseShootPattern();
        }
    }

    // ���� ���� ���� ����
    private void ChooseShootPattern()
    {
        int pattern = Random.Range(0, 4); // 4���� ���� �� �ϳ� ����
        switch (pattern)
        {
            case 0: BasicShoot(); break;
            case 1: TriShot(); break;
            case 2: CircleShoot(); break;
            case 3: MultiShot(); break;
        }
    }

    // �Ѿ� �߻� �Լ�
    private void ShootBullet(Vector2 direction, GameObject bulletprefab)
    {
        if (isDead) return;

        GameObject bullet = Instantiate(bulletprefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = direction * bulletSpeed; // �Ѿ� �ӵ� ����

        // �Ѿ� ���⿡ �°� ȸ�� ����
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        FlipSprite(direction.x);
    }

    // �ִϸ��̼��� ���� ������ ����ϴ� �ڷ�ƾ
    private IEnumerator WaitForShootAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    // �⺻ �߻� (�÷��̾� �������� 1��)
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

    // �￬�� ���� (�߾� + 20��, -20��)
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

    // 360�� �Ѿ� �߻� (16����)
    public void CallCircleShoot()
    {
        rb.linearVelocity = Vector2.zero; // �̵� ����

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

    // ���� ��� (5��)
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

    // ��������Ʈ ���� ��ȯ
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

    // ������ ó��
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
        if (isDead) return; // �̹� �׾����� �������� ����

        isDead = true;
        GetComponent<Collider2D>().enabled = false; // �浹 ��Ȱ��ȭ
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // ���� ȿ�� ����

        animator.Play("Die");

        SceneManager.LoadScene("Scene_ChoiWonje"); // �� ��ȯ
    }

    public void CallExplode()
    {
        StartCoroutine(PlayExplodeAnimation());
    }

    private IEnumerator PlayExplodeAnimation()
    {
        animator.Play("Explode"); // ���� �ִϸ��̼� ����
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // �ִϸ��̼� ������ ���
        Destroy(gameObject); // ���� ����
    }
}