using UnityEngine;
using Unity.Cinemachine;

public class TelePort : MonoBehaviour
{
    public Vector3 targetPosition;
    public GameObject box1;
    public GameObject box2;
    public CinemachineConfiner2D cinemachineConfiner; // Inspector에서 할당

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어를 텔레포트 위치로 이동
            collision.transform.position = targetPosition;

            // 상자 변경
            if (box1 != null && box2 != null)
            {
                box1.SetActive(false);
                box2.SetActive(true);
            }

            // Cinemachine Confiner 변경
            if (cinemachineConfiner != null && box2 != null)
            {
                Collider2D newBounds = box2.GetComponent<Collider2D>();
                if (newBounds != null)
                {
                    cinemachineConfiner.BoundingShape2D = newBounds;
                    //cinemachineConfiner.InvalidateCache(); // 변경 사항 적용
                }
            }
        }
    }
}
