using UnityEngine;

public class ControlOverlay : MonoBehaviour
{
    public Gamemanager Gmscript;

    // 클릭 상태를 추적하는 변수
    public ControlBlock script_ControlBlock;


    private SpriteRenderer spriteRenderer;
    // 활성화된 상태의 스프라이트, 비활성화된 상태의 스프라이트
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    void Start()
    {
        // 컴포넌트 초기화
        spriteRenderer = GetComponent<SpriteRenderer>();


        // 초기 상태 설정 (비활성화 상태로 시작)
        if (spriteRenderer != null && inactiveSprite != null)
        {
            spriteRenderer.sprite = inactiveSprite; // 비활성화 상태에서 스프라이트 설정
            //this.GetComponent<SpriteRenderer>().enabled = false;
        }


    }

    void Update()
    {

        if (Gmscript.ControlitemGet == 1)
        {
            if (script_ControlBlock.isActive == true)
            {

                spriteRenderer.sprite = activeSprite; // 활성화 상태에서 스프라이트 설정
            }
            else
            {
                spriteRenderer.sprite = inactiveSprite; // 비활성화 상태에서 스프라이트 설정
            }

            // 클릭 상태를 반전시킴
            //isActive = !isActive;
        }
    }
}


