using UnityEngine;
using System.Collections;

public class Boss : Monster
{
    [SerializeField] private AudioClip screamSound; // �Ѿ� �߻� �� ����� ����� Ŭ��
    private bool isChasing = false; // �÷��̾ ���� ������ ����

    protected override void Start()
    {
        base.Start();
        audioSource.PlayOneShot(screamSound); // �߻� ���� ���
    }

    // ������Ʈ �Լ�: �� �����Ӹ��� ȣ��
    protected override void Update()
    {
        // �׾��ų� �÷��̾ ���ų� HP�� 0 ������ ��� �������� ����
        if (isDead || player == null || hp <= 0)
            return;

        float distance = Vector2.Distance(transform.position, player.position); // �÷��̾���� �Ÿ� ���

        // ���� ������ �����ϸ� ���� ���� ��ȯ
        if (distance <= shootRange && Mathf.Abs(player.position.y - transform.position.y) <= shootYOffset)
        {
            if (!isChasing) // ���� ���� �ƴϸ� ���� ����
            {
                isChasing = false; // ���� ����
                StopMoving(); // �̵� ���߱�
            }
            Shoot(); // ���� ����
        }
        // �÷��̾ ���� ���� ���� ���� ��
        else if (distance <= followRange)
        {
            if (!isChasing) // ������ ����
            {
                isChasing = true;
            }
            FollowPlayer(); // �÷��̾� ����
        }
        // ���� ���� ���̸� �������� �̵�
        else
        {
            if (isChasing) // ���� ���̸� �̵� ���߱�
            {
                isChasing = false;
                StopMoving();
            }
            RandomMove(); // ���� �̵�
        }
    }

    // �̵��� ���ߴ� �Լ�
    private void StopMoving()
    {
        rb.linearVelocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        animator.Play("Idle"); // �ִϸ��̼�: ��� ����
    }

    // �÷��̾ �����ϴ� �Լ�
    protected override void FollowPlayer()
    {
        if (isDead) return;

        animator.Play("Move"); // �ִϸ��̼�: �̵� ����
        Vector2 direction = (player.position - transform.position).normalized; // �÷��̾� ���� ���
        rb.linearVelocity = direction * moveSpeed; // �̵� �ӵ� ����
        FlipSprite(direction.x); // ���⿡ �°� ��������Ʈ ȸ��
    }

    // ���� �Լ�
    protected override void Shoot()
    {
        if (isDead) return;

        if (Time.time - lastShootTime >= shootInterval) // ���� ���� üũ
        {
            rb.linearVelocity = Vector2.zero; // �̵� ���߱�
            lastShootTime = Time.time; // ������ ���� �ð� ������Ʈ

            // �������� ���� ���� ����
            ChooseShootPattern();
        }
    }

    // ���� ������ �������� �����ϴ� �Լ�
    private void ChooseShootPattern()
    {
        int pattern = Random.Range(0, 4); // 4���� ���� �� ���� ����
        switch (pattern)
        {
            case 0: BasicShoot(); break;  // �⺻ �߻�
            case 1: TriShot(); break;     // �￬�� �߻�
            case 2: CircleShoot(); break; // 360�� �߻�
            case 3: MultiShot(); break;   // ���� �� ���ʴ�� �߻�
        }
    }

    // �ִϸ��̼��� ���� ������ ��ٸ��� �ڷ�ƾ
    private IEnumerator WaitForShootAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // �ִϸ��̼��� ���� ������ ���
        animator.Play("Idle"); // "Idle" �ִϸ��̼����� ��ȯ
    }

    // �Ѿ� �߻� �Լ�
    private void ShootBullet(Vector2 direction)
    {
        if (isDead) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity); // �Ѿ� ����
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = direction * bulletSpeed; // �Ѿ� �ӵ� ����

        // �Ѿ��� ȸ�� ���� ���
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // ȸ�� ����

        FlipSprite(direction.x); // �߻� ���⿡ �°� ��������Ʈ ȸ��
    }

    // �⺻ �߻� �Լ� (�÷��̾� �������� �߻�)
    public void CallBasicShoot()
    {
        audioSource.PlayOneShot(shootSound); // �߻� ���� ���
        ShootBullet((player.position - firePoint.position).normalized); // �÷��̾� �������� �Ѿ� �߻�
    }

    // �⺻ �߻� �ִϸ��̼� ����
    private void BasicShoot()
    {
        animator.Play("CallBasicShoot"); // �ִϸ��̼�: ����
        StartCoroutine(WaitForShootAnimation()); // �ִϸ��̼� �Ϸ� �� idle ���·� ��ȯ
    }

    // �￬�� �߻� �Լ�
    public void CallTriShot()
    {
        audioSource.PlayOneShot(shootSound); // �߻� ���� ���

        // �߾�, 20�� ȸ��, -20�� ȸ�� �� 3�� �߻�
        Vector2 direction = (player.position - firePoint.position).normalized;
        ShootBullet(direction); // �߾� �߻�
        ShootBullet(Quaternion.Euler(0, 0, 20) * direction); // 20�� ȸ���� �������� �߻�
        ShootBullet(Quaternion.Euler(0, 0, -20) * direction); // -20�� ȸ���� �������� �߻�
    }

    // �￬�� �ִϸ��̼� ����
    private void TriShot()
    {
        animator.Play("CallTriShot"); // �ִϸ��̼�: ����
        StartCoroutine(WaitForShootAnimation()); // �ִϸ��̼� �Ϸ� �� idle ���·� ��ȯ
    }

    // 360�� �߻� �Լ� (16�� �߻�)
    private void CircleShoot()
    {
        float originalBulletSpeed = bulletSpeed; // ���� bulletSpeed ����
        bulletSpeed = 0.1f;  // 360�� ���� �߿��� bulletSpeed�� 0.1�� ����

        audioSource.PlayOneShot(screamSound); // �߻� ���� ���

        for (int j = 0; j < 3; j++) // 3�� �ݺ�
        {
            animator.Play("CallCircleShoot"); // �ִϸ��̼�: ���� ����
            WaitForShootAnimation(); // �ִϸ��̼� �Ϸ� ���
        }

        bulletSpeed = originalBulletSpeed;  // ���� bulletSpeed�� ����
        animator.Play("Idle"); // "Idle" �ִϸ��̼����� ��ȯ
    }

    // 360�� �������� �Ѿ� �߻� (16��, 22.5�� ����)
    public void CallCircleShoot()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero; // �̵� ���߱�

        // 360�� �������� �Ѿ� �߻�
        for (int i = 0; i < 16; i++)
        {
            float angle = i * (360f / 16f); // 360���� 16�߷� ������
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)); // ������ �´� ���� ���
            ShootBullet(direction); // �Ѿ� �߻�
        }
    }

    // 5���� ���ʴ�� �߻��ϴ� �Լ�
    public void MultiShot()
    {
        StartCoroutine(MultiShotCoroutine()); // �ڷ�ƾ�� �̿��� 5�� ���� �߻�
    }

    // 5���� ���ʴ�� �߻��ϴ� �ڷ�ƾ
    private IEnumerator MultiShotCoroutine()
    {
        animator.Play("CallMultiShot"); // �ִϸ��̼�: ����
        // 5���� ���������� �߻�
        for (int i = 0; i < 5; i++)
        {
            audioSource.PlayOneShot(shootSound); // �߻� ���� ���
            Vector2 direction = (player.position - firePoint.position).normalized; // �÷��̾� ���� ���
            ShootBullet(direction); // �Ѿ� �߻�
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // �ִϸ��̼� ���
        }

        animator.Play("Idle"); // "Idle" �ִϸ��̼����� ��ȯ
    }

    // ��������Ʈ�� ������ ���ߴ� �Լ�
    protected override void FlipSprite(float directionX)
    {
        if (directionX > 0 && isFacingRight)  // ������ ������ ��
        {
            isFacingRight = false;
            spriteRenderer.flipX = true;
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
        else if (directionX < 0 && !isFacingRight)  // ���� ������ ��
        {
            isFacingRight = true;
            spriteRenderer.flipX = false;
            firePoint.localPosition = new Vector3(-Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
    }

    // �������� �޴� �Լ�
    public override void TakeDamage(int damage)
    {
        if (isDead) return;

        hp -= damage; // HP ����
        if (hp <= 0)
            Die(); // ��� ó��
        else
            flashEffect.Flash(); // ������ ����Ʈ
    }
}