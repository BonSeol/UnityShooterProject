using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public Vector2 targetPosition; // 이동할 위치

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어가 닿았을 때만 실행
        {
            other.transform.position = targetPosition; // 플레이어 위치 변경
        }
    }
}
