using UnityEngine;
using System.Collections;

public class IceBullet : PlayerBullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Monster"))
        {
            Monster monster = collision.GetComponent<Monster>();
            if (monster != null)
            {
                // ���Ͱ� �̹� ���ο� �������� Ȯ��
                if (!monster.IsSlowed)
                {
                    monster.ApplySlowEffect(1.5f, 3f); // 3�� ���� �ӵ� 1.5 ����
                }
            }
        }
    }
}