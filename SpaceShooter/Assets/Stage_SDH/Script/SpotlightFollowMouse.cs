using UnityEngine;

public class SpotlightFollowMouse : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform

    void Update()
    {
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = player.position.z; // Z���� �÷��̾�� �����ϰ� ���� (2D�� ��� �߿�)

        // �÷��̾�� ���콺�� ���� ���͸� ���
        Vector3 direction = mousePos - player.position;

        // ���� ������ ������ Spotlight�� ȸ������ ��ȯ
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ������ -90���� �߰��Ͽ� ������ �������� ����
        angle -= 90;

        // Spotlight�� ȸ�� ������ ����
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
