using UnityEngine;

public class Hole : MonoBehaviour
{
    public int damage = 1;
    public float visibleDuration = 1f;

    void Start()
    {
        // �ش� ��ġ���� ���������� �ִϸ��̼� ��� �� �������
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play(0); // ù ��° ����(�ִϸ��̼�) �ڵ� ���
        }

        Destroy(gameObject, visibleDuration); // �ð� ������ ����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �������� �شٸ� ���⿡ Player�� damage �ִ� �ڵ�
            Debug.Log("Player hit by hole");
        }
    }

    private void OnBecameInvisible()
    {
        // Ȥ�� ī�޶� ������ ������ ��쵵 ����
        Destroy(gameObject);
    }
}
