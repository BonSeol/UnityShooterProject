using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public GameObject target;
    public float Speed = 3f;
    public int HP = 1;
    public int damageToPlayer = 3;

    public Vector2 dir;
    public Vector2 dirNo;
    Animator animator;

    Rigidbody2D pRig2D;
    SpriteRenderer sp;
    private bool isDead = false;
    private int isRight = 1; // 방향 변수 추가


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDead) return;

        dir = target.transform.position - transform.position;
        dirNo = dir.normalized;

        transform.Translate(dirNo * Speed * Time.deltaTime);

        // 플레이어의 위치에 따라 x축 flip 조정
        if ((dir.x < 0 && isRight == 1) || (dir.x > 0 && isRight == -1))
        {
            sp.flipX = sp.flipX == true ? false : true;
            isRight = -isRight;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어에게 데미지를 주는 로직 (추후 구현 필요)
            // collision.GetComponent<Player>().TakeDamage(damageToPlayer);
            Die();
        }
        else if (collision.CompareTag("Bullet"))
        {
            // 총알에 맞으면 데미지를 입음
            TakeDamage(10); // 총알 데미지는 10으로 설정 (필요하면 변경 가능)
            Destroy(collision.gameObject); // 총알 삭제 (필요하면 유지 가능)
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        HP -= damage;
        animator.SetTrigger("Hit");

        if (HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Death");
        Speed = 0f; // 이동 중지
        GetComponent<Collider2D>().enabled = false; // 충돌 제거
        Destroy(gameObject, 1f); // 1초 후 제거 (애니메이션 시간에 맞춰 조정 가능)
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
