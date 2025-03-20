using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // 스피드: 플레이어의 이동 속도
    [SerializeField] private float moveSpeed = 5.0f;

    // 화면 경계를 저장할 변수: 플레이어가 벗어나지 않도록 경계를 설정
    private Vector2 minBounds;
    private Vector2 maxBounds;

    // 애니메이터를 가져올 변수: 애니메이션 제어를 위한 애니메이터 컴포넌트
    Animator animator;

    // 플레이어의 이동 방향
    private Vector2 movement = Vector2.zero;

    void Start()
    {
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트를 가져옴

        Camera cam = Camera.main;
        // Camera.main.ViewportToWorldPoint(): 카메라 뷰포트 좌표(0~1 범위)를 월드 좌표로 변환하는 함수
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)); // 화면의 왼쪽 아래 (bottom-left)
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0)); // 화면의 오른쪽 위 (top-right)

        // 화면 경계 설정: 플레이어가 화면 밖으로 나가지 않도록 제한
        minBounds = new Vector2(bottomLeft.x, bottomLeft.y);
        maxBounds = new Vector2(topRight.x, topRight.y);
    }

    private void Update()
    {
        // 마우스 위치 가져오기
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D이므로 Z값을 0으로 고정

        // 플레이어와 마우스의 방향 계산
        Vector3 direction = mousePos - transform.position;

        // 방향을 Normalized로 정규화하여 크기 조정 (크기 상관없이 방향만 중요)
        direction.Normalize();

        // 마우스 방향에 따른 애니메이션 변경
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
    }

    void FixedUpdate()
    {
        // 방향키에 따른 이동: 이동 속도와 시간을 고려하여 이동
        float moveX = moveSpeed * Time.deltaTime * movement.x; // 좌우 이동
        float moveY = moveSpeed * Time.deltaTime * movement.y; // 상하 이동

        // 새 위치 계산: 현재 위치에 이동 값을 더함
        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);

        // 경계를 벗어나지 않도록 위치 제한
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        // 이동할 경우에만 위치를 업데이트
        if (movement != Vector2.zero)
            transform.position = newPosition;

        // 애니메이션 업데이트
        UpdateAnimation();
    }

    // 애니메이션 상태 업데이트: 이동 중이면 "Player_Walk" 애니메이션, 아니면 "Player_Idle" 애니메이션
    void UpdateAnimation()
    {
        if (movement != Vector2.zero)
        {
            if (movement.y > 0) // 위쪽 이동
            {
                if (movement.x > 0) animator.Play("Pwalk_up_right");
                else if (movement.x < 0) animator.Play("Pwalk_up_left");
                else animator.Play("Pwalk_up");
            }
            else if (movement.y < 0) // 아래쪽 이동
            {
                if (movement.x > 0) animator.Play("Pwalk_down_right");
                else if (movement.x < 0) animator.Play("Pwalk_down_left");
                else animator.Play("Pwalk_down");
            }
            else // 좌우 이동
            {
                if (movement.x > 0) animator.Play("Pwalk_right");
                else animator.Play("Pwalk_left");
            }
        }
        else
        {
            animator.Play("player_idle"); // 가만히 있을 때
        }
    }


    // 입력에 따라 플레이어의 이동 방향 설정
    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>(); // 이동 방향을 벡터로 받아옴
    }
}