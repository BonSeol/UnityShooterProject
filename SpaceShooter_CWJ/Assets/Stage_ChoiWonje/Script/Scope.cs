using UnityEngine;
using UnityEngine.UI;

public class Scope : MonoBehaviour
{
    RectTransform crosshairTransform;
    [SerializeField] Canvas canvas; // UI ĵ���� ����

    void Start()
    {
        crosshairTransform = GetComponent<RectTransform>();
        Cursor.visible = false; // �⺻ ���콺 Ŀ�� �����
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 anchoredPos;

        // ���콺 ��ǥ�� UI ĵ���� ��ǥ�� ��ȯ
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            mousePosition,
            canvas.worldCamera,
            out anchoredPos
        );

        // ��ȯ�� ��ġ ����
        crosshairTransform.anchoredPosition = anchoredPos;
    }
}
