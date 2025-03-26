using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // �÷��̾� �̵� �ӵ�
    [SerializeField] private float moveSpeed = 5.0f;

    // �ִϸ����� ������Ʈ
    Animator animator;

    // �÷��̾� �̵� ����
    private Vector2 movement = Vector2.zero;

    // �� �� �ѱ� ��ġ ������Ʈ
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



    public bool isPlayerinside = false; // ������Ʈ ������ ������ Ȯ���ϴ� ����
    public Collider2D currentObject; // ���� �浹�� ������Ʈ�� ������ ����
    private GameObject currentGun; // ���� ������ ����


    void Start()
    {
        // �ִϸ����� ������Ʈ ��������
        animator = GetComponent<Animator>();

        // ó������ weapon1�� ����
        currentGun = gun1;
        gun1.SetActive(true);
        gun2.SetActive(false);

    }

    private void Update()
    {
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D �����̹Ƿ� Z���� 0���� ����

        // �÷��̾�� ���콺 �� ���� ���� ��� �� ����ȭ
        Vector3 direction = mousePos - transform.position;
        direction.Normalize();

        // ���콺 ���⿡ ���� ȸ�� ���� ���
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // �ִϸ��̼� �Ķ���� ������Ʈ
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        // �� ȸ�� ����
        currentGun.transform.rotation = Quaternion.Euler(0, 0, angle);

        // ���콺 ���⿡ ���� ������ �ѱ� ��ġ ����
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

        // �ѱ� ���� ���� (������ �ٶ� ��� ���� ����)
        currentGun.transform.localScale = mousePos.x < transform.position.x ? new Vector3(1, -1, 1) : new Vector3(1, 1, 1);

        // Ű �Է����� ���� ��ȯ
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
        // �÷��̾� �̵� ó��
        float moveX = moveSpeed * Time.deltaTime * movement.x;
        float moveY = moveSpeed * Time.deltaTime * movement.y;

        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);

        if (movement != Vector2.zero)
            transform.position = newPosition;

        // �ִϸ��̼� ���� ������Ʈ
        UpdateAnimation();
    }

    // �̵� ���¿� ���� �ִϸ��̼� ����
    void UpdateAnimation()
    {
        if (movement != Vector2.zero)
            animator.Play("Player_Walk");
        else
            animator.Play("Player_Idle");
    }

    // Player Input ������Ʈ���� Move �Է��� �����Ǹ� OnMove() �Լ��� �ڵ����� ȣ��
    // �Է°��� ���� �̵� ���� ����
    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    void OnInteract(InputValue Button)
    {
        // ������Ʈ ������ ���Դ���, ������Ʈ�� ����ƴ��� ����üũ
        if (isPlayerinside && currentObject != null)
        {
            Destroy(currentObject.gameObject);
            currentObject = null; // ������ ������Ʈ �ʱ�ȭ 
            isPlayerinside = false;
        }
        Debug.Log("�÷��̾ ��ȣ�ۿ� ��ư�� �������ϴ�123.");
    }


    // Ʈ���� ������ �� �� ȣ��˴ϴ�.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���ŵ� ������Ʈ�� �±׸� Ȯ��
        if (collision.CompareTag("Object"))
        {
            Debug.Log("��ȣ�ۿ� ������ ������Ʈ�� Ʈ���Ű� �߻��߽��ϴ�2.");
            isPlayerinside = true;
            currentObject = collision; //���� ������Ʈ�� ���� 
            Debug.Log(isPlayerinside);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        // Ʈ���ŵ� ������Ʈ�� �±׸� Ȯ��
        if (collision.CompareTag("Object"))
        {
            Debug.Log("��ȣ�ۿ� ������ ������Ʈ�� Ʈ���Ű� �߻��߽��ϴ�3.");
            isPlayerinside = false;
            currentObject = null; // ���� ������ �������Ƿ� ������ ������Ʈ ����
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