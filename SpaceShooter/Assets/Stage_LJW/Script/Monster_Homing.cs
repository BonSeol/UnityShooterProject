using UnityEngine;

public class Monster_Homing : MonoBehaviour
{
    // ���� �̻���
    public GameObject target;   // �÷��̾ ã��

    // �÷��̾ ã�� ������ �ʿ�
    public float Speed = 2f;
    Vector2 dir;    // ����
    Vector2 dirNo;

    void Start()
    {
        // �÷��̾� �±׷� ã��
        target = GameObject.FindGameObjectWithTag("Player");

    }


    void Update()
    {
        // A-B
        dir = target.transform.position - transform.position;
        // ���� ����
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
