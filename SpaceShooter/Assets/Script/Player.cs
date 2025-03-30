using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    // �÷��̾� �̵� �ӵ�
    [SerializeField] private float moveSpeed = 5.0f;

    // �ִϸ����� ������Ʈ
    Animator animator;

    // �÷��̾� HP
    [SerializeField] private int Hp = 5;
    private bool isDead = false; // Die ���¸� ��Ÿ���� �÷���

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



    public bool isPlayerinside_Object = false; // ������Ʈ ������ ������ Ȯ���ϴ� ����
    public bool isPlayerinside_Weapon = false; // ������Ʈ ������ ������ Ȯ���ϴ� ����
    public bool isPlayerinside_Item = false; // ������Ʈ ������ ������ Ȯ���ϴ� ����
    public Collider2D currentObject = null; // ���� �浹�� ������Ʈ�� ������ ����
    public Collider2D currentObject_Weapon = null; // ���� �浹�� ������Ʈ�� ������ ����
    public Collider2D currentObject_Item = null; // ���� �浹�� ������Ʈ�� ������ ����
    private GameObject currentGun; // ���� ������ ����

    public float dashDistance = 5f; // �뽬 �ӵ� 
    public float dashTime = 0.2f;   // �뽬�ϴµ� �ɸ��½ð�
    public float dashCooldown = 3f; // �뽬 ��Ÿ��
    private bool isDashing = false;
    private float lastDashTime = 0f; // ������ �뽬 �ð�

    private float gun1_Active = 0;
    private float gun2_Active = 0;

    public float maxHealth = 10.0f; // �ִ� ü��
    private float currentHealth;

    void Start()
    {
        // �ִϸ����� ������Ʈ ��������
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        // ó������ gun1�� ����
        currentGun = gun1;
        gun1.SetActive(true);
        gun1_Active = 1;
        gun2.SetActive(false);
        gun2_Active = 0;

        //��1,��2 ��Ȯ�� 
        Gun ScriptGun1 = gun1.GetComponent<Gun>();
        ScriptGun1.isPlayerEquip = true;
        Gun ScriptGun2 = gun2.GetComponent<Gun>();
        ScriptGun2.isPlayerEquip = true;

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
        if (isDashing)
            animator.Play("Player_Dash");
        else if (movement != Vector2.zero)
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

    void OnInteract(InputValue value)
    {
        // ������Ʈ ������ ���Դ���, ������Ʈ�� ����ƴ��� ����üũ
        if (isPlayerinside_Object && currentObject != null)
        {
            // ItemDrop ��ũ��Ʈ�� �ִ��� Ȯ���ϰ� Drop() ȣ��
            ItemDrop itemDrop = currentObject.GetComponent<ItemDrop>();
            if (itemDrop != null)
            {
                itemDrop.Drop();  // ������ ��� ����
            }

            // ������Ʈ ���� �� �ʱ�ȭ
            Destroy(currentObject.gameObject);
            currentObject = null;
            isPlayerinside_Object = false;
        }


        if (isPlayerinside_Item && currentObject_Item != null)
        {
            if (currentObject_Item.gameObject.name == "Small_Hp")
            {
                currentHealth += 0.5f;
                maxHealth += 0.5f;

            }
            else if (currentObject_Item.gameObject.name == "Big_Hp")
            {
                currentHealth += 1.5f;
                maxHealth += 1.5f;

            }

            // ������Ʈ ���� �� �ʱ�ȭ
            Destroy(currentObject_Item.gameObject);
            currentObject = null;
            isPlayerinside_Object = false;
        }

        // ���� ��ȣ�ۿ� ó��
        if (isPlayerinside_Weapon && currentObject_Weapon != null)
        {
            if (gun1_Active == 1 && gun2_Active == 0)
            {
                // ���� ���� ������
                Gun ScriptGun = currentGun.GetComponent<Gun>();
                ScriptGun.isPlayerEquip = false;
                currentGun.tag = "Weapon";
                Instantiate(currentGun, transform.position, Quaternion.identity);

                // ������Ʈ�� ����� ����
                GameObject newWeapon = Instantiate(currentObject_Weapon.gameObject, transform.position, Quaternion.identity);
                Destroy(currentObject_Weapon.gameObject);
                gun1.SetActive(false);

                // ���ο� ����� ����
                gun1 = newWeapon;
                gun1.tag = "Gun";
                Gun ScriptGun1 = gun1.GetComponent<Gun>();
                ScriptGun1.isPlayerEquip = true;
                gun1.SetActive(false);
                Debug.Log("���⸦ �����߽��ϴ�: " + currentGun.name);

                // ���� ������Ʈ �ʱ�ȭ
                currentObject_Weapon = null; // ������ ������Ʈ �ʱ�ȭ
                isPlayerinside_Weapon = false; // ���� �ʱ�ȭ
            }
            else if (gun1_Active == 0 && gun2_Active == 1)
            {
                // ���� ���� ������
                Gun ScriptGun = currentGun.GetComponent<Gun>();
                ScriptGun.isPlayerEquip = false;
                currentGun.tag = "Weapon";
                Instantiate(currentGun, transform.position, Quaternion.identity);

                // ������Ʈ�� ����� ����
                GameObject newWeapon = Instantiate(currentObject_Weapon.gameObject, transform.position, Quaternion.identity);
                Destroy(currentObject_Weapon.gameObject);
                gun2.SetActive(false);

                // ���ο� ����� ����
                gun2 = newWeapon;
                gun2.tag = "Gun";
                Gun ScriptGun2 = gun2.GetComponent<Gun>();
                ScriptGun2.isPlayerEquip = true;
                gun2.SetActive(false);
                Debug.Log("���⸦ �����߽��ϴ�: " + currentGun.name);

                // ������Ʈ �ʱ�ȭ
                currentObject_Weapon = null;
                isPlayerinside_Weapon = false; // ���� �ʱ�ȭ
            }
        }
    }


    void OnSprint(InputValue value)
    {
        // �뽬�ϴ� ���� Ȯ�� 
        if (value.isPressed && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            //�ڷ�ƾ���� �뽬 ��� 
            StartCoroutine(Dash());
        }
        // �뽬 ��Ÿ�� 
        if (value.isPressed && !isDashing && Time.time < lastDashTime + dashCooldown)
        {
            float remainingTime = (lastDashTime + dashCooldown) - Time.time;
            Debug.Log(remainingTime.ToString("F2") + "�� ���ҽ��ϴ�.");
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���ŵ� ������Ʈ�� �±׸� Ȯ��
        if (collision.CompareTag("Object"))
        {
            Debug.Log("��ȣ�ۿ� ������ ������Ʈ�� Ʈ���Ű� �߻��߽��ϴ�.");
            isPlayerinside_Object = true;
            currentObject = collision;
        }
        else if (collision.CompareTag("Weapon"))
        {
            Debug.Log("��ȣ�ۿ� ������ ������Ʈ�� Ʈ���Ű� �߻��߽��ϴ�2.");
            isPlayerinside_Weapon = true;       // ������Ʈ ���� Ȯ��
            currentObject_Weapon = collision;   // ���� ������Ʈ�� ����
        }
        else if (collision.CompareTag("Item"))
        {
            Debug.Log("��ȣ�ۿ� ������ ������Ʈ�� Ʈ���Ű� �߻��߽��ϴ�2.");
            isPlayerinside_Item = true;       // ������Ʈ ���� Ȯ��
            currentObject_Item = collision;   // ���� ������Ʈ�� ����
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        // Ʈ���ŵ� ������Ʈ�� �±׸� Ȯ��
        if (collision.CompareTag("Object"))
        {
            Debug.Log("������Ʈ �������� �������ϴ�.");
            isPlayerinside_Object = false;
            currentObject = null;
        }
        // Ʈ���ŵ� ������Ʈ�� �±׸� Ȯ��
        else if (collision.CompareTag("Weapon"))
        {
            Debug.Log("���� �������� �������ϴ�.");
            isPlayerinside_Weapon = false;
            currentObject_Weapon = null;
        }
        else if (collision.CompareTag("Item"))
        {
            Debug.Log("������ �������� �������ϴ�");
            isPlayerinside_Item = false;       // ������Ʈ ���� Ȯ��
            currentObject_Item = collision;   // ���� ������Ʈ�� ����
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
        lastDashTime = Time.time;
        animator.Play("Player_Dash");

        Vector3 dashDirection = new Vector3(movement.x, movement.y).normalized;
        Vector3 startPosition = transform.position;
        float maxDistance = dashDistance;

        Vector3 targetPosition = startPosition + dashDirection * maxDistance;

        float elapsedTime = 0f;

        while (elapsedTime < dashTime)
        {
            Vector3 nextPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / dashTime);

            // Collider �浹 ����
            if (Physics2D.OverlapCircle(nextPosition, 0.2f, LayerMask.GetMask("Wall")))
            {
                break; // �浹�ϸ� ��� ��� ����
            }

            transform.position = nextPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("���ظ� �Ծ����ϴ�");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        /*
        isDead = true;  // Die ���·� ����
        GetComponent<Collider2D>().isTrigger = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // ���� ����� ���� (������ ���߱� ����)
        
        // animator.Play("Die");
        // Destroy(gameObject, 0.2f);
        */
    }


}