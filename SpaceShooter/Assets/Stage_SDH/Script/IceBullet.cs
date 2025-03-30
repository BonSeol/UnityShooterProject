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
                // 몬스터가 이미 슬로우 상태인지 확인
                if (!monster.IsSlowed)
                {
                    monster.ApplySlowEffect(1.5f, 3f); // 3초 동안 속도 1.5 감소
                }
            }
        }
    }
}