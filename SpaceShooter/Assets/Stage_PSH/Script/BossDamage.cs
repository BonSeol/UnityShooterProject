using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public int damage = 10;  // 오브젝트의 피해량

    // 충돌시 호출되는 함수
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌했을 때
        if (other.CompareTag("Player"))
        {
            // 플레이어의 TakeDamage() 메서드를 호출하여 데미지 입힘
            other.GetComponent<Player>()?.TakeDamage(damage);

            // 오브젝트를 삭제 (처음 충돌 후)
            Destroy(gameObject);
        }
    }
}
