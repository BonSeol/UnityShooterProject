using UnityEngine;
using UnityEngine.UI;

public class Scope : MonoBehaviour
{
    RectTransform crosshairTransform;
    [SerializeField] Canvas canvas; // UI 캔버스 참조

    void Start()
    {
        crosshairTransform = GetComponent<RectTransform>();
        Cursor.visible = false; // 기본 마우스 커서 숨기기
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 anchoredPos;

        // 마우스 좌표를 UI 캔버스 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            mousePosition,
            canvas.worldCamera,
            out anchoredPos
        );

        // 변환된 위치 적용
        crosshairTransform.anchoredPosition = anchoredPos;
    }
}
