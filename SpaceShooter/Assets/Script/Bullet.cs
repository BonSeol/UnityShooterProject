using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;  // �Ѿ��� ���� �ð�
    public float Damage = 1.0f;

    void Start()
    {
        // ���� �ð� �� �Ѿ� ����
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���Ϳ� �浹�ߴ��� Ȯ��
        if (collision.CompareTag("Monster"))
        {
            Monster_Golem monster = collision.GetComponent<Monster_Golem>();
            Boss_Golem boss = collision.GetComponent<Boss_Golem>();
            if (monster != null)
            {
                monster.TakeDamage(Damage); // ���Ϳ��� ������ ������
            }
            else if (boss != null)
            {
                boss.TakeDamage(Damage); // ���Ϳ��� ������ ������
            }
            Destroy(gameObject); // �Ѿ� ����
        }
    }
}
