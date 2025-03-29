using UnityEngine;

public class PBullet : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 10f;
    public float lifetime = 2f;

    void Start()
    {
        // 총알 발사 후 lifetime 시간이 지나면 제거
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 총알을 앞쪽 방향으로 이동
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌 시 총알 제거
        Destroy(gameObject);
    }
}