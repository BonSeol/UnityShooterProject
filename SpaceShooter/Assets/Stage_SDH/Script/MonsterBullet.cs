using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    public float lifeTime = 5f;  // �Ѿ��� ���� �ð�
    Rigidbody2D rb;
    public float speed = 10f;
    public int damage = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // �Ѿ��� �߻� �������� �̵��ϵ��� ����
        rb.linearVelocity = transform.right * speed;

        // ���� �ð� �� �Ѿ� ����
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // �Ѿ��� �浹�� ��ü�� ���� ó�� (��: ������ �ֱ�)
        if (collision.CompareTag("Player"))
        {
            rb.linearVelocity = Vector2.zero;

            // Player�� TakeDamage ȣ��
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(10); // �⺻ ������ 10
            }

            // �Ѿ��� �����ΰ��� �浹�ϸ� �Ѿ� ����
            Destroy(gameObject, 0.2f);
        }

        // ���� �¾��� ���
        else if (collision.CompareTag("Wall"))
        {
            rb.linearVelocity = Vector2.zero;

            // �ִϸ��̼� ���� �� ����
            Destroy(gameObject, 0.2f);
        }
    }
}
