using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public Vector2 targetPosition; // �̵��� ��ġ

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // �÷��̾ ����� ���� ����
        {
            other.transform.position = targetPosition; // �÷��̾� ��ġ ����
        }
    }
}
