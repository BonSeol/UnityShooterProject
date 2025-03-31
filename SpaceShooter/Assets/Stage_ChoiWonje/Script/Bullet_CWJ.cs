using UnityEngine;

public class Bullet_CWJ : MonoBehaviour
{
    public float lifeTime = 5f;  // 총알의 생명 시간
    public bool interaction;
    void Start()
    {
        // 일정 시간 후 총알 삭제
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // 총알이 충돌한 물체에 대해 처리 (예: 데미지 주기)
        if (other.tag == "Enemy")
        {
            // 적에 충돌했을 경우
            //Debug.Log("Enemy Hit!");
        }

        // 총알이 무엇인가에 충돌하면 총알 삭제
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // 총알이 무엇인가에 충돌하면 총알 삭제
        Destroy(gameObject);
    }
}
