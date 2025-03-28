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

    // 플래시 이펙트
    [SerializeField] protected FlashEffect flashEffect;

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

    // 오디오 관련 변수
    [SerializeField] private AudioClip gemPickupSound; // Gem 획득 사운드
    [SerializeField] private AudioClip altarSound; // 재단 활성화 사운드
    private AudioSource audioSource;

    private bool isPlayerinside = false; // 오브젝트 구역에 들어갔는지 확인하는 변수
    public Collider2D currentObject; // 현재 충돌한 오브젝트를 저장할 변수
    private GameObject currentGun; // 현재 장착된 무기
                                   // 트리거 영역에 들어갈 때 호출됩니다.
    Rigidbody2D rb;

    [SerializeField] private int hp = 300;
    private bool isDead = false; // Die 상태를 나타내는 플래그

    [SerializeField] private int gemCount = 0; // 보석 개수 체크
    [SerializeField] private int AltarCount = 0; // 재단 개수 체크

    void Start()
    {
        // 애니메이터 컴포넌트 가져오기
        animator = GetComponent<Animator>();

        // 처음에는 weapon1을 장착
        currentGun = gun1;
        gun1.SetActive(true);
        gun2.SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // AudioSource 가져오기
        flashEffect = GetComponent<FlashEffect>();
    }

    private void Update()
    {
        rb.linearVelocity = Vector2.zero; // 이동 멈춤 (몬스터한테 밀림 방지)

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
        // 오브젝트 구역에 들어왔는지, 오브젝트가 저장됐는지 이중체크
        if (isPlayerinside && currentObject != null)
        {
            if (currentObject.CompareTag("Gem"))
            {
                gemCount++;
                isPlayerinside = false;
                audioSource.PlayOneShot(gemPickupSound);
                Destroy(currentObject.gameObject);
            }
            else if (currentObject.CompareTag("Altar") && gemCount > 0)
            {
                // Altar의 불꽃 활성화 함수 호출
                Altar altarScript = currentObject.GetComponent<Altar>();
                if (altarScript != null)
                {
                    AltarCount++;
                    gemCount--;
                    audioSource.PlayOneShot(altarSound);
                    altarScript.ActivateFlame(); // 불꽃 활성화
                }

                isPlayerinside = false;
                Destroy(currentObject.gameObject);
            }
            else if (currentObject.CompareTag("Door") && AltarCount >= 4)
            {
                isPlayerinside = false;
                transform.position = new Vector2(94.5f, 84f);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        hp -= damage;
        if (hp <= 0)
            Die();
        else
            flashEffect.Flash();
    }

    private void Die()
    {
        isDead = true;  // Die 상태로 설정
        GetComponent<Collider2D>().isTrigger = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // 물리 계산을 멈춤 (완전히 멈추기 위함)
        animator.Play("Die");

        // Die 애니메이션 끝난 후 Explode 애니메이션 재생
        StartCoroutine(PlayExplodeAnimation());
    }

    private IEnumerator PlayExplodeAnimation()
    {
        // Die 애니메이션 끝날 때까지 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        // 0.8초 대기
        yield return new WaitForSeconds(0.8f);

        // Explode 애니메이션 시작
        animator.Play("Explode");

        // Explode 애니메이션 끝난 후 몬스터 삭제
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 특정 태그(Gem, Altar, Door)에 들어갔는지 확인
        if (collision.CompareTag("Gem") || collision.CompareTag("Altar") || collision.CompareTag("Door"))
        {
            isPlayerinside = true;
            currentObject = collision;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // 특정 태그에서 벗어났는지 확인
        if (collision.CompareTag("Gem") || collision.CompareTag("Altar") || collision.CompareTag("Door"))
        {
            isPlayerinside = false;
            currentObject = null;
        }

        // 몬스터와의 충돌이 끝남 -> 더 이상 밀리지 않도록 설정
        if (collision.CompareTag("Monster"))
        {
            rb.linearVelocity = Vector2.zero; // 속도를 0으로 만들어 멈추게 함
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