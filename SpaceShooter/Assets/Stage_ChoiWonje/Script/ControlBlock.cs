using UnityEngine;

public class ControlBlock : MonoBehaviour
{
    public Gamemanager Gmrscript;
    // Ŭ�� ���¸� �����ϴ� ����
    public bool isActive = false;

    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    // Ȱ��ȭ�� ������ ��������Ʈ, ��Ȱ��ȭ�� ������ ��������Ʈ
    public Sprite activeSprite;
    public Sprite inactiveSprite;



    void Start()
    {
        // ������Ʈ �ʱ�ȭ
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        // �ʱ� ���� ���� (��Ȱ��ȭ ���·� ����)
        if (spriteRenderer != null && inactiveSprite != null)
        {
            spriteRenderer.sprite = inactiveSprite; // ��Ȱ��ȭ ���¿��� ��������Ʈ ����

        }

        if (polygonCollider != null)
        {
            polygonCollider.enabled = true; // ������ �ݶ��̴� Ȱ��ȭ

        }
    }

    void Update()
    {
        if (Gmrscript.ControlitemGet == 1)
        {
            // ���콺 ���� ��ư Ŭ�� ��
            if (Input.GetMouseButtonDown(1)) // 1�� ��Ŭ��
            {
                // Ŭ�� ���¿� ���� ���� ����
                if (isActive)
                {
                    // ���°� 'Ȱ��'�̸�
                    //Debug.Log("��Ŭ���Ͽ� ��Ȱ��ȭ��");

                    // ��������Ʈ ��Ȱ��ȭ ���·� ����
                    if (spriteRenderer != null && inactiveSprite != null)
                    {
                        spriteRenderer.sprite = inactiveSprite;
                    }

                    // �±׸� 'Inactive'�� ����
                    gameObject.tag = "Wall";


                    // ������ �ݶ��̴� ��Ȱ��ȭ
                    if (polygonCollider != null)
                    {
                        polygonCollider.enabled = true;
                    }
                }
                else
                {
                    // ���°� '��Ȱ��'�̸�
                    //Debug.Log("��Ŭ���Ͽ� Ȱ��ȭ��");

                    // ��������Ʈ Ȱ��ȭ ���·� ����
                    if (spriteRenderer != null && activeSprite != null)
                    {
                        spriteRenderer.sprite = activeSprite;
                    }

                    // �±׸� 'Active'�� ����
                    gameObject.tag = "Untagged";

                    // ������ �ݶ��̴� ��Ȱ��ȭ
                    if (polygonCollider != null)
                    {
                        polygonCollider.enabled = false;
                    }
                }

                // Ŭ�� ���¸� ������Ŵ
                isActive = !isActive;
            }
        }
    }
}



