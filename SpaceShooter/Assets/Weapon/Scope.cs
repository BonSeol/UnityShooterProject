using UnityEngine;
// using UnityEngine.UI;

public class Scope : MonoBehaviour
{
    RectTransform crosshairTransform;

    void Start()
    {
        crosshairTransform = GetComponent<RectTransform>();
        Cursor.visible = false; // �⺻ ���콺 Ŀ�� �����
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        crosshairTransform.position = mousePosition;
    }
}