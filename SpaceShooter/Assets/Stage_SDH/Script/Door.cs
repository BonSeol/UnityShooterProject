using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    private bool isPlayerinside_Object = false; // 오브젝트 구역에 들어갔는지 확인하는 변수

    void OnInteract(InputValue value)
    {
        // 오브젝트 구역에 들어왔는지 체크
        if (isPlayerinside_Object && GameManager.Instance.AltarCount == 4)
        {
            Transform playerTransform = GameObject.FindWithTag("Player")?.transform;
            if (playerTransform != null)
            {
                playerTransform.position = new Vector2(94.5f, 84f);
            }
            isPlayerinside_Object = false;
            
        }
    }

    // 트리거 영역에 들어갈 때 호출됩니다.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerinside_Object = true;
        }
    }

    // 벗어났는지 확인
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerinside_Object = false;
        }
    }
}
