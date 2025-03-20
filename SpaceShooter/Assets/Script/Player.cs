using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // ���ǵ�: �÷��̾��� �̵� �ӵ�
    [SerializeField] private float moveSpeed = 5.0f;

    // ȭ�� ��踦 ������ ����: �÷��̾ ����� �ʵ��� ��踦 ����
    private Vector2 minBounds;
    private Vector2 maxBounds;

    // �ִϸ����͸� ������ ����: �ִϸ��̼� ��� ���� �ִϸ����� ������Ʈ
    Animator animator;

    // �÷��̾��� �̵� ����
    private Vector2 movement = Vector2.zero;

    void Start()
    {
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ�� ������

        Camera cam = Camera.main;
        // Camera.main.ViewportToWorldPoint(): ī�޶� ����Ʈ ��ǥ(0~1 ����)�� ���� ��ǥ�� ��ȯ�ϴ� �Լ�
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)); // ȭ���� ���� �Ʒ� (bottom-left)
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0)); // ȭ���� ������ �� (top-right)

        // ȭ�� ��� ����: �÷��̾ ȭ�� ������ ������ �ʵ��� ����
        minBounds = new Vector2(bottomLeft.x, bottomLeft.y);
        maxBounds = new Vector2(topRight.x, topRight.y);
    }

    private void Update()
    {
        // ���콺 ��ġ ��������
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D�̹Ƿ� Z���� 0���� ����

        // �÷��̾�� ���콺�� ���� ���
        Vector3 direction = mousePos - transform.position;

        // ������ Normalized�� ����ȭ�Ͽ� ũ�� ���� (ũ�� ������� ���⸸ �߿�)
        direction.Normalize();

        // ���콺 ���⿡ ���� �ִϸ��̼� ����
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
    }

    void FixedUpdate()
    {
        // ����Ű�� ���� �̵�: �̵� �ӵ��� �ð��� ����Ͽ� �̵�
        float moveX = moveSpeed * Time.deltaTime * movement.x; // �¿� �̵�
        float moveY = moveSpeed * Time.deltaTime * movement.y; // ���� �̵�

        // �� ��ġ ���: ���� ��ġ�� �̵� ���� ����
        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);

        // ��踦 ����� �ʵ��� ��ġ ����
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        // �̵��� ��쿡�� ��ġ�� ������Ʈ
        if (movement != Vector2.zero)
            transform.position = newPosition;

        // �ִϸ��̼� ������Ʈ
        UpdateAnimation();
    }

    // �ִϸ��̼� ���� ������Ʈ: �̵� ���̸� "Player_Walk" �ִϸ��̼�, �ƴϸ� "Player_Idle" �ִϸ��̼�
    void UpdateAnimation()
    {
        if (movement != Vector2.zero)
        {
            if (movement.y > 0) // ���� �̵�
            {
                if (movement.x > 0) animator.Play("Pwalk_up_right");
                else if (movement.x < 0) animator.Play("Pwalk_up_left");
                else animator.Play("Pwalk_up");
            }
            else if (movement.y < 0) // �Ʒ��� �̵�
            {
                if (movement.x > 0) animator.Play("Pwalk_down_right");
                else if (movement.x < 0) animator.Play("Pwalk_down_left");
                else animator.Play("Pwalk_down");
            }
            else // �¿� �̵�
            {
                if (movement.x > 0) animator.Play("Pwalk_right");
                else animator.Play("Pwalk_left");
            }
        }
        else
        {
            animator.Play("player_idle"); // ������ ���� ��
        }
    }


    // �Է¿� ���� �÷��̾��� �̵� ���� ����
    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>(); // �̵� ������ ���ͷ� �޾ƿ�
    }
}