using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;  // �Ѿ� ������
    public float bulletSpeed = 10f;  // �Ѿ� �ӵ�
    public GameObject firePos;  // �Ѿ� �߻� ��ġ

    void Update()
    {
        // ���콺 ��ġ�� �������� �� ȸ��
        AimAtMouse();

        // ���콺 ���� ��ư Ŭ�� �� ���� ���
        if (Input.GetMouseButtonDown(0)) // ���� ���콺 ��ư
        {
            Fire();
        }
    }

    void AimAtMouse()
    {
        // ���콺 ��ġ�� 3D �������� ��ȯ
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.z * -1f;  // ī�޶�� ���� ���̷� ����
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // ���� ���콺�� ���ϵ��� ȸ��
        Vector3 direction = worldMousePos - transform.position;
        direction.z = 0f;  // 2D �����̹Ƿ� z�� ����

        // ���� ���͸� ������ ��ȯ �� ����
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // �Ѿ� �߻�
    void Fire()
    {
        // firePos���� �Ѿ� ����
        GameObject bullet = Instantiate(bulletPrefab, firePos.transform.position, transform.rotation);

        // �Ѿ˿� ���� ���Ͽ� �߻�
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.z * -1f;
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

            // �Ѿ��� ���콺�� ���ϵ��� ���� ���
            Vector2 fireDirection = (worldMousePos - firePos.transform.position).normalized;

            // �Ѿ� �߻�
            bulletRb.linearVelocity = fireDirection * bulletSpeed;
        }
    }


}
