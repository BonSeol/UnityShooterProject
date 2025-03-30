using UnityEngine;

public class SpotlightFollowMouse : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform

    void Update()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = player.position.z; // Z값을 플레이어와 동일하게 설정 (2D일 경우 중요)

        // 플레이어와 마우스의 방향 벡터를 계산
        Vector3 direction = mousePos - player.position;

        // 방향 벡터의 각도를 Spotlight의 회전으로 변환
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 각도에 -90도를 추가하여 정면을 기준으로 맞춤
        angle -= 90;

        // Spotlight의 회전 각도를 설정
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
