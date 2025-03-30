using UnityEngine;
using Unity.Cinemachine;

public class TelePort : MonoBehaviour
{
    public Vector3 targetPosition;
    public GameObject box1;
    public GameObject box2;
    public CinemachineConfiner2D cinemachineConfiner; // Inspector���� �Ҵ�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �÷��̾ �ڷ���Ʈ ��ġ�� �̵�
            collision.transform.position = targetPosition;

            // ���� ����
            if (box1 != null && box2 != null)
            {
                box1.SetActive(false);
                box2.SetActive(true);
            }

            // Cinemachine Confiner ����
            if (cinemachineConfiner != null && box2 != null)
            {
                Collider2D newBounds = box2.GetComponent<Collider2D>();
                if (newBounds != null)
                {
                    cinemachineConfiner.BoundingShape2D = newBounds;
                    //cinemachineConfiner.InvalidateCache(); // ���� ���� ����
                }
            }
        }
    }
}
