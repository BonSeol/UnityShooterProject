using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Gem : MonoBehaviour
{
    [SerializeField] private AudioClip gemPickupSound; // Gem ȹ�� ����
    private AudioSource audioSource;
    private bool isPlayerinside_Object = false; // ������Ʈ ������ ������ Ȯ���ϴ� ����

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnInteract(InputValue value)
    {
        // ������Ʈ ������ ���Դ��� üũ
        if (isPlayerinside_Object)
        {
            GameManager.Instance.gemCount++; // �̱����� ���� GameManager�� ����
            isPlayerinside_Object = false;

            // �Ҹ��� �����Ǿ� �ִٸ� ���
            if (gemPickupSound != null)
                audioSource.PlayOneShot(gemPickupSound);
     
            Destroy(gameObject, 0.2f);
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
