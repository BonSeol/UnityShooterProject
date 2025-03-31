using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public GameObject menuUI;  // ESC로 여닫는 메뉴 UI
    public Button exitButton1;  // EXIT 버튼
    public Button exitButton2;  // EXIT 버튼
    public Button exitButton3;  // EXIT 버튼

    public AudioSource audioSource; // 오디오 소스
    public AudioClip menuOpenSound; // 메뉴 열릴 때 소리
    public AudioClip buttonClickSound; // 버튼 클릭 소리

    void Start()
    {
        menuUI.SetActive(false); // 시작할 때 메뉴 숨김
        exitButton1.onClick.AddListener(ReturnToMainMenu);
        exitButton2.onClick.AddListener(ReturnToMainMenu);
        exitButton3.onClick.AddListener(ReturnToMainMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuUI.SetActive(!menuUI.activeSelf);
            if (audioSource != null && menuOpenSound != null)
            {
                audioSource.PlayOneShot(menuOpenSound);
            }
        }
    }



    // 메인 메뉴로 이동
    private void ReturnToMainMenu()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }

        // 0.3초 후 씬 전환 (소리 들린 후 이동)
        Invoke("LoadMainMenu", 0.3f);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("StartMenuScene");
    }
}
