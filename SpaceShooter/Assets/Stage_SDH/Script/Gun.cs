using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // �߻��� �Ѿ� ������
    [SerializeField] private float bulletSpeed = 10f; // �Ѿ� �ӵ�
    [SerializeField] private GameObject firePos; // �Ѿ��� �߻�� ��ġ
    [SerializeField] private AudioClip shootSound; // �߻� �Ҹ�

    private AudioSource audioSource; // ����� �ҽ� ������Ʈ

    void Start()
    {
        // ����� �ҽ� ������Ʈ ��������
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // ���콺 �������� �ѱ� ����
        AimAtMouse();

        // ���콺 ���� ��ư Ŭ�� �� �߻�
        if (Input.GetMouseButtonDown(0))
            Fire();
    }

    // ���콺 �������� �ѱ� ȸ��
    void AimAtMouse()
    {
        Vector3 mousePos = Input.mousePosition; // ���콺 ��ġ ��������
        mousePos.z = Camera.main.transform.position.z * -1f; // ī�޶� ���� Z ��ġ ����
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos); // ȭ�� ��ǥ -> ���� ��ǥ ��ȯ

        Vector3 direction = worldMousePos - transform.position; // �ѱ����� ���콺������ ���� ���� ���
        direction.z = 0f; // 2D ȯ���̹Ƿ� Z�� �� 0���� ����

        // ���� ���͸� ������ ��ȯ�Ͽ� �ѱ� ȸ�� ����
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // �Ѿ� �߻� �Լ�
    void Fire()
    {
        // �Ѿ� ���� (�ѱ� ��ġ���� ���� ȸ�� �������� ����)
        GameObject bullet = Instantiate(bulletPrefab, firePos.transform.position, transform.rotation);

        // �Ѿ��� Rigidbody2D ��������
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            // ���콺 ��ġ�� �������� �Ѿ� �߻� ���� ����
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.z * -1f;
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector2 fireDirection = (worldMousePos - firePos.transform.position).normalized; // ����ȭ�� ���� ����

            // �Ѿ˿� �ӵ� �����Ͽ� �߻�
            bulletRb.linearVelocity = fireDirection * bulletSpeed;
        }

        // �߻� �Ҹ��� �����Ǿ� �ִٸ� ���
        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }
}
