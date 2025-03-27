using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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



    public bool isPlayerinside_Object = false; // 오브젝트 구역에 들어갔는지 확인하는 변수
    public bool isPlayerinside_Weapon = false; // 오브젝트 구역에 들어갔는지 확인하는 변수
    public Collider2D currentObject; // 현재 충돌한 오브젝트를 저장할 변수
    public Collider2D currentObject_Weapon; // 현재 충돌한 오브젝트를 저장할 변수
    private GameObject currentGun; // 현재 장착된 무기

    public float dashDistance = 5f; // 대쉬 속도 
    public float dashTime = 0.2f;   // 대쉬하는데 걸리는시간
    public float dashCooldown = 3f; // 대쉬 쿨타임
    private bool isDashing = false;
    private float lastDashTime = 0f; // 마지막 대쉬 시간

    private float gun1_Active = 0;
    private float gun2_Active = 0;

    void Start()
    {
        // 애니메이터 컴포넌트 가져오기
        animator = GetComponent<Animator>();

        // 처음에는 gun1을 장착
        currentGun = gun1;
        gun1.SetActive(true);
        gun1_Active = 1;
        gun2.SetActive(false);
        gun2_Active = 0;

        //총1,총2 총확인 
        Gun ScriptGun1 = gun1.GetComponent<Gun>();
        ScriptGun1.isPlayerEquip = true;
        Gun ScriptGun2 = gun2.GetComponent<Gun>();
        ScriptGun2.isPlayerEquip = true;

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
        if (isDashing)
            animator.Play("Player_Dash");
        else if (movement != Vector2.zero)
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

    void OnInteract(InputValue value)
    {
        // 오브젝트 구역에 들어왔는지, 오브젝트를 저장됐는지 이중체크
        if (isPlayerinside_Object && currentObject != null)
        {
            Destroy(currentObject.gameObject);
            currentObject = null; // 저장한 오브젝트 초기화 
            isPlayerinside_Object = false; 
        }


        if (isPlayerinside_Weapon && currentObject_Weapon != null)
        {
            //무기 확인 
            if (gun1_Active == 1 && gun2_Active ==0 )
            {
                //기존 무기 버리기 
                Gun ScriptGun = currentGun.GetComponent<Gun>();                     
                ScriptGun.isPlayerEquip = false;                                                        
                currentGun.tag = "Weapon";                                                              
                Instantiate(currentGun, transform.position, Quaternion.identity);   
                
                // 오브젝트를 무기로 장착
                GameObject newWeapon = Instantiate(currentObject_Weapon.gameObject, transform.position, Quaternion.identity);
                Destroy(currentObject_Weapon.gameObject);
                gun1.SetActive(false);


                // 새로운 무기로 설정
                gun1 = newWeapon; 
                gun1.tag = "Gun";
                Gun ScriptGun1 = gun1.GetComponent<Gun>();
                ScriptGun1.isPlayerEquip = true;
                gun1.SetActive(false);
                Debug.Log("무기를 장착했습니다: " + currentGun.name);

                // 현재 오브젝트 초기화              
                currentObject_Weapon = null; // 저장한 오브젝트 초기화 
                isPlayerinside_Object = false;
            }
            if (gun1_Active == 0 && gun2_Active == 1)
            {
                // 기존 무기 버리기 
                Gun ScriptGun = currentGun.GetComponent<Gun>();
                ScriptGun.isPlayerEquip = false;
                currentGun.tag = "Weapon";
                Instantiate(currentGun, transform.position, Quaternion.identity);

                // 오브젝트를 무기로 장착
                GameObject newWeapon = Instantiate(currentObject_Weapon.gameObject, transform.position, Quaternion.identity);
                Destroy(currentObject_Weapon.gameObject);
                gun2.SetActive(false);

                // 새로운 무기로 설정
                gun2 = newWeapon;
                gun2.tag = "Gun";
                Gun ScriptGun2 = gun2.GetComponent<Gun>();
                ScriptGun2.isPlayerEquip = true;
                gun2.SetActive(false);
                Debug.Log("무기를 장착했습니다: " + currentGun.name);

                // 오브젝트 초기화
                currentObject_Weapon = null;
                isPlayerinside_Weapon = false; // 상태 초기화
            }
        }
    }

    void OnSprint(InputValue value)
    {
        // 대쉬하는 조건 확인 
        if (value.isPressed && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            //코루틴으로 대쉬 사용 
            StartCoroutine(Dash());
        }
        // 대쉬 쿨타임 
        if (value.isPressed && !isDashing && Time.time < lastDashTime + dashCooldown)
        {
            float remainingTime = (lastDashTime + dashCooldown) - Time.time;
            Debug.Log(remainingTime.ToString("F2") + "초 남았습니다.");
        }

    }


    // 트리거 영역에 들어갈 때 호출됩니다.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // 트리거된 오브젝트의 태그를 확인
        if (collision.CompareTag("Object"))
        {
            Debug.Log("상호작용 가능한 오브젝트와 트리거가 발생했습니다2.");
            isPlayerinside_Object = true;       //오브젝트 구역 확인
            currentObject = collision;          //현재 오브젝트에 저장 
            Debug.Log(isPlayerinside_Object);
        }

        else if(collision.CompareTag("Weapon"))
        {
            Debug.Log("상호작용 가능한 오브젝트와 트리거가 발생했습니다2.");
            isPlayerinside_Weapon = true;       //오브젝트 구역 확인
            currentObject_Weapon = collision;          //현재 오브젝트에 저장 
            Debug.Log(isPlayerinside_Weapon);
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        // 트리거된 오브젝트의 태그를 확인
        if (collision.CompareTag("Object"))
        {
            Debug.Log("상호작용 가능한 오브젝트와 트리거가 발생했습니다3.");
            isPlayerinside_Object = false;     // 오브젝트 구역 확인 
            currentObject = null;       // 구역 밖으로 나갔으므로 저장한 오브젝트 삭제
            Debug.Log(isPlayerinside_Object);
        }
        // 트리거된 오브젝트의 태그를 확인
        else if (collision.CompareTag("Weapon"))
        {
            Debug.Log("상호작용 가능한 오브젝트와 트리거가 발생했습니다2.");
            isPlayerinside_Weapon = false;       //오브젝트 구역 확인
            currentObject_Weapon = null;          //현재 오브젝트에 저장 
            Debug.Log(isPlayerinside_Weapon);
        }

    }


    void SwitchGun(int gunNumber)
    {
        if (gunNumber == 1)
        {
            currentGun.SetActive(false);
            gun2_Active = 0;
            currentGun = gun1;
            currentGun.SetActive(true);
            gun1_Active = 1;
        }
        else if (gunNumber == 2)
        {
            if(gun2!= null)
            {
                currentGun.SetActive(false);
                gun1_Active = 0; 
                currentGun = gun2;
                currentGun.SetActive(true);
                gun2_Active = 1;
            }
            return;
        }
    }



    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;   // 대쉬를 사용한 시간을 기록
        animator.Play("Player_Idle");


        Vector3 dashDirection = new Vector3(movement.x, movement.y).normalized;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + dashDirection * dashDistance;

        float elapsedTime = 0f;

        while (elapsedTime < dashTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / dashTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        isDashing = false;
    }

}