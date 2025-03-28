using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // 발사할 총알 프리팹
    [SerializeField] private float bulletSpeed = 10f; // 총알 속도
    [SerializeField] private GameObject firePos; // 총알이 발사될 위치
    [SerializeField] private AudioClip shootSound; // 발사 소리

    private AudioSource audioSource; // 오디오 소스 컴포넌트

    void Start()
    {
        // 오디오 소스 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 마우스 방향으로 총구 조준
        AimAtMouse();

        // 마우스 왼쪽 버튼 클릭 시 발사
        if (Input.GetMouseButtonDown(0))
            Fire();
    }

    // 마우스 방향으로 총구 회전
    void AimAtMouse()
    {
        Vector3 mousePos = Input.mousePosition; // 마우스 위치 가져오기
        mousePos.z = Camera.main.transform.position.z * -1f; // 카메라 기준 Z 위치 보정
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos); // 화면 좌표 -> 월드 좌표 변환

        Vector3 direction = worldMousePos - transform.position; // 총구에서 마우스까지의 방향 벡터 계산
        direction.z = 0f; // 2D 환경이므로 Z축 값 0으로 설정

        // 방향 벡터를 각도로 변환하여 총구 회전 적용
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 총알 발사 함수
    void Fire()
    {
        // 총알 생성 (총구 위치에서 현재 회전 방향으로 생성)
        GameObject bullet = Instantiate(bulletPrefab, firePos.transform.position, transform.rotation);

        // 총알의 Rigidbody2D 가져오기
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            // 마우스 위치를 기준으로 총알 발사 방향 설정
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.z * -1f;
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector2 fireDirection = (worldMousePos - firePos.transform.position).normalized; // 정규화된 방향 벡터

            // 총알에 속도 적용하여 발사
            bulletRb.linearVelocity = fireDirection * bulletSpeed;
        }

        // 발사 소리가 설정되어 있다면 재생
        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }
}
