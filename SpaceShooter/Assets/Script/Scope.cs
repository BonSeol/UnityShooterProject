using UnityEngine;
// using UnityEngine.UI;

public class Scope : MonoBehaviour
{
    RectTransform crosshairTransform;

    void Start()
    {
        crosshairTransform = GetComponent<RectTransform>();
        Cursor.visible = false; // 기본 마우스 커서 숨기기
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        crosshairTransform.position = mousePosition;
    }
}