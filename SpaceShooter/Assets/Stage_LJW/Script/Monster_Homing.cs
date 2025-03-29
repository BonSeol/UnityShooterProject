using UnityEngine;

public class Monster_Homing : MonoBehaviour
{
    // 유도 미사일
    public GameObject target;   // 플레이어를 찾기

    // 플레이어를 찾는 로직이 필요
    public float Speed = 2f;
    Vector2 dir;    // 방향
    Vector2 dirNo;

    void Start()
    {
        // 플레이어 태그로 찾기
        target = GameObject.FindGameObjectWithTag("Player");

    }


    void Update()
    {
        // A-B
        dir = target.transform.position - transform.position;
        // 방향 벡터
        dirNo = dir.normalized;

        transform.Translate(dirNo * Speed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
