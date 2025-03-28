using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    private bool isPlayerinside_Object = false; // ������Ʈ ������ ������ Ȯ���ϴ� ����

    void OnInteract(InputValue value)
    {
        // ������Ʈ ������ ���Դ��� üũ
        if (isPlayerinside_Object && GameManager.Instance.AltarCount == 4)
        {
            Transform playerTransform = GameObject.FindWithTag("Player")?.transform;
            if (playerTransform != null)
            {
                playerTransform.position = new Vector2(94.5f, 84f);
            }
            isPlayerinside_Object = false;
            
        }
    }

    // Ʈ���� ������ �� �� ȣ��˴ϴ�.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerinside_Object = true;
        }
    }

    // ������� Ȯ��
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerinside_Object = false;
        }
    }
}
