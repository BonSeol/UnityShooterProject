using UnityEngine;
using UnityEngine.Audio;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;  // 총알 프리팹
    [SerializeField] private float bulletSpeed = 10f;  // 총알 속도
    [SerializeField] private GameObject firePos;  // 총알 발사 위치
    public bool isPlayerEquip = false; //총 여부를 확인
    [SerializeField] private Player Playerscript;
    [SerializeField] private GameObject Playerobject;
    [SerializeField] private int gunLayer = 0;
    [SerializeField] private bool player_interaction;
    [SerializeField] private AudioClip shootSound; // 발사 소리
    private AudioSource audioSource; // 오디오 소스 컴포넌트

    void Start()
    {
        // 오디오 소스 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        gunLayer = Playerscript.gunLayer;

        if (gunLayer == 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = Playerobject.gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        else if (gunLayer == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = Playerobject.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }

        if (isPlayerEquip == true)
        {
            // 마우스 위치를 기준으로 총 회전
            AimAtMouse();

            // 마우스 왼쪽 버튼 클릭 시 총을 쏘기
            if (Input.GetMouseButtonDown(0)) // 왼쪽 마우스 버튼
            {
                if (player_interaction == false)
                    Fire();
            }
        }
    }

    void AimAtMouse()
    {
        // 마우스 위치를 3D 공간으로 변환
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.z * -1f;  // 카메라와 같은 깊이로 설정
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // 총이 마우스를 향하도록 회전
        Vector3 direction = worldMousePos - transform.position;
        direction.z = 0f;  // 2D 게임이므로 z축 고정

        // 방향 벡터를 각도로 변환 후 적용
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 총알 발사
    void Fire()
    {
        // firePos에서 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, firePos.transform.position, transform.rotation);

        // 총알에 힘을 가하여 발사
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            // 마우스 위치를 월드 좌표로 변환
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.z * -1f;
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

            // 총알이 마우스를 향하도록 벡터 계산
            Vector2 fireDirection = (worldMousePos - firePos.transform.position).normalized;

            // 총알 발사
            bulletRb.linearVelocity = fireDirection * bulletSpeed;

            // 발사 소리가 설정되어 있다면 재생
            if (shootSound != null)
                audioSource.PlayOneShot(shootSound);
        }
    }
}