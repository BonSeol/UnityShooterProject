using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public string sceneName = ""; // 이동할 씬 이름 (유니티 인스펙터에서 설정)
    public AudioClip clickSound; // 버튼 클릭 사운드
    private AudioSource audioSource;
    private Button button;
    private Color originalColor;

    void Start()
    {
        // 버튼과 오디오 초기화
        button = GetComponent<Button>();
        audioSource = gameObject.AddComponent<AudioSource>();

        if (clickSound != null)
            audioSource.clip = clickSound;

        // 버튼 원래 색상 저장
        originalColor = button.image.color;

        // 버튼 클릭 이벤트 등록
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // 버튼 색 어둡게 변경
        button.image.color = Color.gray;

        // 클릭 소리 재생
        if (clickSound != null)
            audioSource.Play();

        // 버튼 기능 실행
        if (sceneName != "")
        {
            // 씬 이동
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            // 게임 종료
            QuitGame();
        }
    }

    void QuitGame()
    {
        // 에디터에서 실행 중이면 멈추기
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 빌드된 게임에서는 종료
#endif
    }
}
