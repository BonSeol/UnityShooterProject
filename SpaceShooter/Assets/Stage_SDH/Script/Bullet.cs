using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;  // 총알의 생명 시간

    void Start()
    {
        // 일정 시간 후 총알 삭제
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 총알이 충돌한 물체에 대해 처리 (예: 데미지 주기)
        if (collision.collider.CompareTag("Enemy"))
        {
            // 적에 충돌했을 경우
            Debug.Log("Enemy Hit!");
        }

        // 총알이 무엇인가에 충돌하면 총알 삭제
        Destroy(gameObject);
    }
}
