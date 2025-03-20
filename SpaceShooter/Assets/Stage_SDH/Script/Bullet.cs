using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;  // �Ѿ��� ���� �ð�

    void Start()
    {
        // ���� �ð� �� �Ѿ� ����
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // �Ѿ��� �浹�� ��ü�� ���� ó�� (��: ������ �ֱ�)
        if (collision.collider.CompareTag("Enemy"))
        {
            // ���� �浹���� ���
            Debug.Log("Enemy Hit!");
        }

        // �Ѿ��� �����ΰ��� �浹�ϸ� �Ѿ� ����
        Destroy(gameObject);
    }
}
