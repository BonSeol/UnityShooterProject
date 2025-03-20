using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // 플레이어 이동 속도
    [SerializeField] private float moveSpeed = 5.0f;

    // 화면 경계 좌표 (플레이어가 화면을 벗어나지 않도록 제한)
    private Vector2 minBounds;
    private Vector2 maxBounds;

    // 애니메이터 컴포넌트
    Animator animator;

    // 플레이어 이동 방향
    private Vector2 movement = Vector2.zero;

    // 총 및 총구 위치 오브젝트
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject gunPos_Right;
    [SerializeField] private GameObject gunPos_Left;
    [SerializeField] private GameObject gunPos_Up;
    [SerializeField] private GameObject gunPos_UpL;
    [SerializeField] private GameObject gunPos_UpR;
    [SerializeField] private GameObject gunPos_DownL;
    [SerializeField] private GameObject gunPos_DownR;
    [SerializeField] private GameObject gunPos_UpRight;
    [SerializeField] private GameObject gunPos_UpLeft;
    [SerializeField] private GameObject gunPos_DownLeft;
    [SerializeField] private GameObject gunPos_DownRight;

    void Start()
    {
        // 애니메이터 컴포넌트 가져오기
        animator = GetComponent<Animator>();

        // 화면의 경계를 월드 좌표로 변환하여 설정
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        minBounds = new Vector2(bottomLeft.x, bottomLeft.y);
        maxBounds = new Vector2(topRight.x, topRight.y);
    }

    private void Update()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D 게임이므로 Z값을 0으로 고정

        // 플레이어와 마우스 간 방향 벡터 계산 및 정규화
        Vector3 direction = mousePos - transform.position;
        direction.Normalize();

        // 마우스 방향에 따른 회전 각도 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 애니메이션 파라미터 업데이트
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        // 총 회전 설정
        gun.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 마우스 방향에 따라 적절한 총구 위치 설정
        Vector3 gunPosition = transform.position;

        if (angle > -22.5f && angle <= 22.5f) gunPosition = gunPos_Right.transform.position;
        else if (angle > 22.5f && angle <= 67.5f) gunPosition = gunPos_UpRight.transform.position;
        else if (angle > 67.5f && angle <= 90f) gunPosition = gunPos_UpR.transform.position;
        else if (angle > 90f && angle <= 112.5f) gunPosition = gunPos_UpL.transform.position;
        else if (angle > 112.5f && angle <= 157.5f) gunPosition = gunPos_UpLeft.transform.position;
        else if (angle > 157.5f || angle <= -157.5f) gunPosition = gunPos_Left.transform.position;
        else if (angle > -157.5f && angle <= -112.5f) gunPosition = gunPos_DownLeft.transform.position;
        else if (angle > -112.5f && angle <= -90f) gunPosition = gunPos_DownL.transform.position;
        else if (angle > -90f && angle <= -67.5f) gunPosition = gunPos_DownR.transform.position;
        else if (angle > -67.5f && angle <= -22.5f) gunPosition = gunPos_DownRight.transform.position;

        gun.transform.position = gunPosition;

        // 총기 방향 반전 (왼쪽을 바라볼 경우 반전 적용)
        gun.transform.localScale = mousePos.x < transform.position.x ? new Vector3(1, -1, 1) : new Vector3(1, 1, 1);
    }

    void FixedUpdate()
    {
        // 플레이어 이동 처리
        float moveX = moveSpeed * Time.deltaTime * movement.x;
        float moveY = moveSpeed * Time.deltaTime * movement.y;

        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);

        // 화면 경계 내에서만 이동하도록 제한
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        if (movement != Vector2.zero)
            transform.position = newPosition;

        // 애니메이션 상태 업데이트
        UpdateAnimation();
    }

    // 이동 상태에 따른 애니메이션 변경
    void UpdateAnimation()
    {
        if (movement != Vector2.zero)
            animator.Play("Player_Walk");
        else
            animator.Play("Player_Idle");
    }

    // Player Input 컴포넌트에서 Move 입력이 감지되면 OnMove() 함수를 자동으로 호출
    // 입력값을 통해 이동 방향 설정
    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }
}