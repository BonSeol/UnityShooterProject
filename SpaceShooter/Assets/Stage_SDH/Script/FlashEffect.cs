using System.Collections;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    [SerializeField] private Material flashMaterial; // ������ ȿ���� ����� ��Ƽ����
    [SerializeField] private float duration; // ������ ���� �ð�

    private SpriteRenderer spriteRenderer; // ������ ȿ���� ������ ��������Ʈ ������
    private Material originalMaterial; // ���� ��� ���� ��Ƽ���� ����
    private Coroutine flashRoutine; // ���� ���� ���� �ڷ�ƾ

    void Start()
    {
        // ���� ������Ʈ�� SpriteRenderer ������Ʈ�� ������
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ���� ����ϴ� ��Ƽ���� ���� (������ ȿ�� �� �����ϱ� ����)
        originalMaterial = spriteRenderer.material;
    }

    public void Flash()
    {
        // ���� �̹� ������ �ڷ�ƾ�� ���� ���̶�� ����
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);
        
        // ���ο� ������ �ڷ�ƾ ���� �� ����
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // ������ ��Ƽ����� ����
        spriteRenderer.material = flashMaterial;

        // ������ ���� �ð���ŭ ���
        yield return new WaitForSeconds(duration);

        // ���� ��Ƽ����� ����
        spriteRenderer.material = originalMaterial;

        // �ڷ�ƾ ���� �Ϸ� ǥ��
        flashRoutine = null;
    }
}
