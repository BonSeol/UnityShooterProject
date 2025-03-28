using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // 싱글톤 인스턴스

    public int gemCount = 0; // 보석 개수 체크
    public int AltarCount = 0; // 재단 개수 체크

    void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복된 GameManager가 생성되면 삭제
            return;
        }

        DontDestroyOnLoad(gameObject); // 씬이 변경되어도 GameManager 유지
    }
}
