using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // �÷��̾� �̵� �ӵ�
    [SerializeField] private float moveSpeed = 5.0f;

    // ȭ�� ��� ��ǥ (�÷��̾ ȭ���� ����� �ʵ��� ����)
    private Vector2 minBounds;
    private Vector2 maxBounds;

    // �ִϸ����� ������Ʈ
    Animator animator;

    // �÷��̾� �̵� ����
    private Vector2 movement = Vector2.zero;

    // �� �� �ѱ� ��ġ ������Ʈ
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
        // �ִϸ����� ������Ʈ ��������
        animator = GetComponent<Animator>();

        // ȭ���� ��踦 ���� ��ǥ�� ��ȯ�Ͽ� ����
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        minBounds = new Vector2(bottomLeft.x, bottomLeft.y);
        maxBounds = new Vector2(topRight.x, topRight.y);
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
        gun.transform.rotation = Quaternion.Euler(0, 0, angle);

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

        gun.transform.position = gunPosition;

        // �ѱ� ���� ���� (������ �ٶ� ��� ���� ����)
        gun.transform.localScale = mousePos.x < transform.position.x ? new Vector3(1, -1, 1) : new Vector3(1, 1, 1);
    }

    void FixedUpdate()
    {
        // �÷��̾� �̵� ó��
        float moveX = moveSpeed * Time.deltaTime * movement.x;
        float moveY = moveSpeed * Time.deltaTime * movement.y;

        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);

        // ȭ�� ��� �������� �̵��ϵ��� ����
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

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
}