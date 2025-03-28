using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    // �÷��̾� �̵� �ӵ�
    [SerializeField] private float moveSpeed = 5.0f;

    // �ִϸ����� ������Ʈ
    Animator animator;

    // �÷��̾� �̵� ����
    private Vector2 movement = Vector2.zero;

    // �÷��� ����Ʈ
    [SerializeField] protected FlashEffect flashEffect;

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

    // ����� ���� ����
    [SerializeField] private AudioClip gemPickupSound; // Gem ȹ�� ����
    [SerializeField] private AudioClip altarSound; // ��� Ȱ��ȭ ����
    private AudioSource audioSource;

    private bool isPlayerinside = false; // ������Ʈ ������ ������ Ȯ���ϴ� ����
    public Collider2D currentObject; // ���� �浹�� ������Ʈ�� ������ ����
    private GameObject currentGun; // ���� ������ ����
                                   // Ʈ���� ������ �� �� ȣ��˴ϴ�.
    Rigidbody2D rb;

    [SerializeField] private int hp = 300;
    private bool isDead = false; // Die ���¸� ��Ÿ���� �÷���

    [SerializeField] private int gemCount = 0; // ���� ���� üũ
    [SerializeField] private int AltarCount = 0; // ��� ���� üũ

    void Start()
    {
        // �ִϸ����� ������Ʈ ��������
        animator = GetComponent<Animator>();

        // ó������ weapon1�� ����
        currentGun = gun1;
        gun1.SetActive(true);
        gun2.SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // AudioSource ��������
        flashEffect = GetComponent<FlashEffect>();
    }

    private void Update()
    {
        rb.linearVelocity = Vector2.zero; // �̵� ���� (�������� �и� ����)

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
            if (currentObject.CompareTag("Gem"))
            {
                gemCount++;
                isPlayerinside = false;
                audioSource.PlayOneShot(gemPickupSound);
                Destroy(currentObject.gameObject);
            }
            else if (currentObject.CompareTag("Altar") && gemCount > 0)
            {
                // Altar�� �Ҳ� Ȱ��ȭ �Լ� ȣ��
                Altar altarScript = currentObject.GetComponent<Altar>();
                if (altarScript != null)
                {
                    AltarCount++;
                    gemCount--;
                    audioSource.PlayOneShot(altarSound);
                    altarScript.ActivateFlame(); // �Ҳ� Ȱ��ȭ
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
        isDead = true;  // Die ���·� ����
        GetComponent<Collider2D>().isTrigger = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // ���� ����� ���� (������ ���߱� ����)
        animator.Play("Die");

        // Die �ִϸ��̼� ���� �� Explode �ִϸ��̼� ���
        StartCoroutine(PlayExplodeAnimation());
    }

    private IEnumerator PlayExplodeAnimation()
    {
        // Die �ִϸ��̼� ���� ������ ���
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        // 0.8�� ���
        yield return new WaitForSeconds(0.8f);

        // Explode �ִϸ��̼� ����
        animator.Play("Explode");

        // Explode �ִϸ��̼� ���� �� ���� ����
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Ư�� �±�(Gem, Altar, Door)�� ������ Ȯ��
        if (collision.CompareTag("Gem") || collision.CompareTag("Altar") || collision.CompareTag("Door"))
        {
            isPlayerinside = true;
            currentObject = collision;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Ư�� �±׿��� ������� Ȯ��
        if (collision.CompareTag("Gem") || collision.CompareTag("Altar") || collision.CompareTag("Door"))
        {
            isPlayerinside = false;
            currentObject = null;
        }

        // ���Ϳ��� �浹�� ���� -> �� �̻� �и��� �ʵ��� ����
        if (collision.CompareTag("Monster"))
        {
            rb.linearVelocity = Vector2.zero; // �ӵ��� 0���� ����� ���߰� ��
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