using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public string sceneName = ""; // �̵��� �� �̸� (����Ƽ �ν����Ϳ��� ����)
    public AudioClip clickSound; // ��ư Ŭ�� ����
    private AudioSource audioSource;
    private Button button;
    private Color originalColor;

    void Start()
    {
        // ��ư�� ����� �ʱ�ȭ
        button = GetComponent<Button>();
        audioSource = gameObject.AddComponent<AudioSource>();

        if (clickSound != null)
            audioSource.clip = clickSound;

        // ��ư ���� ���� ����
        originalColor = button.image.color;

        // ��ư Ŭ�� �̺�Ʈ ���
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // ��ư �� ��Ӱ� ����
        button.image.color = Color.gray;

        // Ŭ�� �Ҹ� ���
        if (clickSound != null)
            audioSource.Play();

        // ��ư ��� ����
        if (sceneName != "")
        {
            // �� �̵�
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            // ���� ����
            QuitGame();
        }
    }

    void QuitGame()
    {
        // �����Ϳ��� ���� ���̸� ���߱�
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ����� ���ӿ����� ����
#endif
    }
}
