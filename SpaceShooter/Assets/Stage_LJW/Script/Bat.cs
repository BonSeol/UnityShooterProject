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
    private int isRight = 1; // ���� ���� �߰�


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

        // �÷��̾��� ��ġ�� ���� x�� flip ����
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
            // �÷��̾�� �������� �ִ� ���� (���� ���� �ʿ�)
            // collision.GetComponent<Player>().TakeDamage(damageToPlayer);
            Die();
        }
        else if (collision.CompareTag("Bullet"))
        {
            // �Ѿ˿� ������ �������� ����
            TakeDamage(10); // �Ѿ� �������� 10���� ���� (�ʿ��ϸ� ���� ����)
            Destroy(collision.gameObject); // �Ѿ� ���� (�ʿ��ϸ� ���� ����)
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
        Speed = 0f; // �̵� ����
        GetComponent<Collider2D>().enabled = false; // �浹 ����
        Destroy(gameObject, 1f); // 1�� �� ���� (�ִϸ��̼� �ð��� ���� ���� ����)
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
