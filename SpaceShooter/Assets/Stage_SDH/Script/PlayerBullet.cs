using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // �Ѿ��� �ӵ��� ���� ��
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 10;

    // �ִϸ����Ϳ� ������ٵ�2D ������Ʈ
    protected Animator animator;
    protected Rigidbody2D rb;

    // �ʱ�ȭ �Լ�
    protected virtual void Start()
    {
        // �ִϸ����Ϳ� ������ٵ�2D ������Ʈ ��������
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // �߻� �ִϸ��̼� ���
        animator.Play("Fire");

        // ���� �ð� �� �̵� �ִϸ��̼����� ����
        Invoke(nameof(StartMoving), 0.1f);

        // �Ѿ��� �߻� �������� �̵��ϵ��� ����
        rb.linearVelocity = transform.right * speed;
    }

    // �Ѿ��� �̵��� �����ϸ� ȣ��Ǵ� �Լ�
    protected void StartMoving()
    {
        // �̵� �ִϸ��̼����� ����
        animator.Play("Move");
    }

    // �浹 ó�� �Լ�
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // ���Ϳ� �浹�� ���
        if (collision.CompareTag("Monster"))
        {
            // ��Ʈ �ִϸ��̼� ���
            animator.Play("Hit");

            // �浹 �� �Ѿ� ����
            rb.linearVelocity = Vector2.zero;

            // ������ TakeDamage �Լ� ȣ���Ͽ� ���� �ֱ�
            Monster monster = collision.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }

            // 0.2�� �� �Ѿ� ��ü�� �ı�
            Destroy(gameObject, 0.2f);
        }

        // ���� �浹�� ���
        else if (collision.CompareTag("Wall"))
        {
            // ��Ʈ �ִϸ��̼� ���
            animator.Play("Hit");

            // �浹 �� �Ѿ� ����
            rb.linearVelocity = Vector2.zero;

            // 0.2�� �� �Ѿ� ��ü�� �ı�
            Destroy(gameObject, 0.2f);
        }
    }
}
