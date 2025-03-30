using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;  // 총알의 생명 시간
    public float Damage = 1.0f;

    void Start()
    {
        // 일정 시간 후 총알 삭제
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 몬스터와 충돌했는지 확인
        if (collision.CompareTag("Monster"))
        {
            Monster_Golem monster = collision.GetComponent<Monster_Golem>();
            Boss_Golem boss = collision.GetComponent<Boss_Golem>();
            if (monster != null)
            {
                monster.TakeDamage(Damage); // 몬스터에게 데미지 입히기
            }
            else if (boss != null)
            {
                boss.TakeDamage(Damage); // 몬스터에게 데미지 입히기
            }
            Destroy(gameObject); // 총알 삭제
        }
    }
}
