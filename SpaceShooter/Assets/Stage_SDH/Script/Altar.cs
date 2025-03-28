using UnityEngine;
using UnityEngine.InputSystem;

public class Altar : MonoBehaviour
{
    [SerializeField] private GameObject flameObject; // �Ҳ� ������Ʈ
    [SerializeField] private AudioClip altarSound; // ��� Ȱ��ȭ ����
    private AudioSource audioSource;
    private bool isPlayerinside_Object = false; // ������Ʈ ������ ������ Ȯ���ϴ� ����

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // �Ҳ� Ȱ��ȭ �Լ�
    public void ActivateFlame()
    {
        if (flameObject != null)
            flameObject.SetActive(true); // �Ҳ� ������Ʈ Ȱ��ȭ
    }

    void OnInteract(InputValue value)
    {
        // ������Ʈ ������ ���Դ���üũ
        if (isPlayerinside_Object)
        {
            if (GameManager.Instance.gemCount > 0)
            {
                ActivateFlame();
                GameManager.Instance.AltarCount++;
                GameManager.Instance.gemCount--;
                isPlayerinside_Object = false;
                audioSource.PlayOneShot(altarSound);
                Destroy(gameObject, 1.5f);
            }
        }
    }

    // Ʈ���� ������ �� �� ȣ��˴ϴ�.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Ư�� �±�(Gem, Altar, Door)�� ������ Ȯ��
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