using UnityEngine;

public class ControlOverlay : MonoBehaviour
{
    public Gamemanager Gmscript;

    // Ŭ�� ���¸� �����ϴ� ����
    public ControlBlock script_ControlBlock;


    private SpriteRenderer spriteRenderer;
    // Ȱ��ȭ�� ������ ��������Ʈ, ��Ȱ��ȭ�� ������ ��������Ʈ
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    void Start()
    {
        // ������Ʈ �ʱ�ȭ
        spriteRenderer = GetComponent<SpriteRenderer>();


        // �ʱ� ���� ���� (��Ȱ��ȭ ���·� ����)
        if (spriteRenderer != null && inactiveSprite != null)
        {
            spriteRenderer.sprite = inactiveSprite; // ��Ȱ��ȭ ���¿��� ��������Ʈ ����
            //this.GetComponent<SpriteRenderer>().enabled = false;
        }


    }

    void Update()
    {

        if (Gmscript.ControlitemGet == 1)
        {
            if (script_ControlBlock.isActive == true)
            {

                spriteRenderer.sprite = activeSprite; // Ȱ��ȭ ���¿��� ��������Ʈ ����
            }
            else
            {
                spriteRenderer.sprite = inactiveSprite; // ��Ȱ��ȭ ���¿��� ��������Ʈ ����
            }

            // Ŭ�� ���¸� ������Ŵ
            //isActive = !isActive;
        }
    }
}


