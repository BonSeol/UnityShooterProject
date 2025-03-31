using UnityEngine;

public class Bullet_CWJ : MonoBehaviour
{
    public float lifeTime = 5f;  // �Ѿ��� ���� �ð�
    public bool interaction;
    void Start()
    {
        // ���� �ð� �� �Ѿ� ����
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // �Ѿ��� �浹�� ��ü�� ���� ó�� (��: ������ �ֱ�)
        if (other.tag == "Enemy")
        {
            // ���� �浹���� ���
            //Debug.Log("Enemy Hit!");
        }

        // �Ѿ��� �����ΰ��� �浹�ϸ� �Ѿ� ����
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // �Ѿ��� �����ΰ��� �浹�ϸ� �Ѿ� ����
        Destroy(gameObject);
    }
}
