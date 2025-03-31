using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public int damage = 10;  // ������Ʈ�� ���ط�

    // �浹�� ȣ��Ǵ� �Լ�
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾�� �浹���� ��
        if (other.CompareTag("Player"))
        {
            // �÷��̾��� TakeDamage() �޼��带 ȣ���Ͽ� ������ ����
            other.GetComponent<Player>()?.TakeDamage(damage);

            // ������Ʈ�� ���� (ó�� �浹 ��)
            Destroy(gameObject);
        }
    }
}
