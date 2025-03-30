using UnityEngine;

public class ControlBlock : MonoBehaviour
{
    public Gamemanager Gmrscript;
    // 클릭 상태를 추적하는 변수
    public bool isActive = false;

    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    // 활성화된 상태의 스프라이트, 비활성화된 상태의 스프라이트
    public Sprite activeSprite;
    public Sprite inactiveSprite;



    void Start()
    {
        // 컴포넌트 초기화
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        // 초기 상태 설정 (비활성화 상태로 시작)
        if (spriteRenderer != null && inactiveSprite != null)
        {
            spriteRenderer.sprite = inactiveSprite; // 비활성화 상태에서 스프라이트 설정

        }

        if (polygonCollider != null)
        {
            polygonCollider.enabled = true; // 폴리곤 콜라이더 활성화

        }
    }

    void Update()
    {
        if (Gmrscript.ControlitemGet == 1)
        {
            // 마우스 왼쪽 버튼 클릭 시
            if (Input.GetMouseButtonDown(1)) // 1은 우클릭
            {
                // 클릭 상태에 따라 동작 결정
                if (isActive)
                {
                    // 상태가 '활성'이면
                    //Debug.Log("우클릭하여 비활성화됨");

                    // 스프라이트 비활성화 상태로 변경
                    if (spriteRenderer != null && inactiveSprite != null)
                    {
                        spriteRenderer.sprite = inactiveSprite;
                    }

                    // 태그를 'Inactive'로 변경
                    gameObject.tag = "Wall";


                    // 폴리곤 콜라이더 비활성화
                    if (polygonCollider != null)
                    {
                        polygonCollider.enabled = true;
                    }
                }
                else
                {
                    // 상태가 '비활성'이면
                    //Debug.Log("우클릭하여 활성화됨");

                    // 스프라이트 활성화 상태로 변경
                    if (spriteRenderer != null && activeSprite != null)
                    {
                        spriteRenderer.sprite = activeSprite;
                    }

                    // 태그를 'Active'로 변경
                    gameObject.tag = "Untagged";

                    // 폴리곤 콜라이더 비활성화
                    if (polygonCollider != null)
                    {
                        polygonCollider.enabled = false;
                    }
                }

                // 클릭 상태를 반전시킴
                isActive = !isActive;
            }
        }
    }
}



