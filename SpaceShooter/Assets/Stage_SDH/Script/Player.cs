using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // 플레이어 이동 속도
    [SerializeField] private float moveSpeed = 5.0f;

    // 애니메이터 컴포넌트
    Animator animator;

    // 플레이어 이동 방향
    private Vector2 movement = Vector2.zero;

    // 총 및 총구 위치 오브젝트
    [SerializeField] private GameObject gun1;
    [SerializeField] private GameObject gun2;
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



    public bool isPlayerinside = false; // 오브젝트 구역에 들어갔는지 확인하는 변수
    public Collider2D currentObject; // 현재 충돌한 오브젝트를 저장할 변수
    private GameObject currentGun; // 현재 장착된 무기


    void Start()
    {
        // 애니메이터 컴포넌트 가져오기
        animator = GetComponent<Animator>();

        // 처음에는 weapon1을 장착
        currentGun = gun1;
        gun1.SetActive(true);
        gun2.SetActive(false);

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
        currentGun.transform.rotation = Quaternion.Euler(0, 0, angle);

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

        currentGun.transform.position = gunPosition;

        // 총기 방향 반전 (왼쪽을 바라볼 경우 반전 적용)
        currentGun.transform.localScale = mousePos.x < transform.position.x ? new Vector3(1, -1, 1) : new Vector3(1, 1, 1);

        // 키 입력으로 무기 전환
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchGun(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchGun(2);
        }
    }

    void FixedUpdate()
    {
        // 플레이어 이동 처리
        float moveX = moveSpeed * Time.deltaTime * movement.x;
        float moveY = moveSpeed * Time.deltaTime * movement.y;

        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);

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

    void OnInteract(InputValue Button)
    {
        // 오브젝트 구역에 들어왔는지, 오브젝트를 저장됐는지 이중체크
        if (isPlayerinside && currentObject != null)
        {
            Destroy(currentObject.gameObject);
            currentObject = null; // 저장한 오브젝트 초기화 
            isPlayerinside = false;
        }
        Debug.Log("플레이어가 상호작용 버튼을 눌렀습니다123.");
    }


    // 트리거 영역에 들어갈 때 호출됩니다.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // 트리거된 오브젝트의 태그를 확인
        if (collision.CompareTag("Object"))
        {
            Debug.Log("상호작용 가능한 오브젝트와 트리거가 발생했습니다2.");
            isPlayerinside = true;
            currentObject = collision; //현재 오브젝트에 저장 
            Debug.Log(isPlayerinside);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        // 트리거된 오브젝트의 태그를 확인
        if (collision.CompareTag("Object"))
        {
            Debug.Log("상호작용 가능한 오브젝트와 트리거가 발생했습니다3.");
            isPlayerinside = false;
            currentObject = null; // 구역 밖으로 나갔으므로 저장한 오브젝트 삭제
            Debug.Log(isPlayerinside);
        }
    }




    void SwitchGun(int gunNumber)
    {
        if (gunNumber == 1)
        {
            currentGun.SetActive(false);
            currentGun = gun1;
            currentGun.SetActive(true);
        }
        else if (gunNumber == 2)
        {
            currentGun.SetActive(false);
            currentGun = gun2;
            currentGun.SetActive(true);
        }
    }


}