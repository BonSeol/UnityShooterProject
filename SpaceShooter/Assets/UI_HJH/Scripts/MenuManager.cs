using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public GameObject menuUI;  // ESC�� ���ݴ� �޴� UI
    public Button exitButton1;  // EXIT ��ư
    public Button exitButton2;  // EXIT ��ư
    public Button exitButton3;  // EXIT ��ư

    public AudioSource audioSource; // ����� �ҽ�
    public AudioClip menuOpenSound; // �޴� ���� �� �Ҹ�
    public AudioClip buttonClickSound; // ��ư Ŭ�� �Ҹ�

    void Start()
    {
        menuUI.SetActive(false); // ������ �� �޴� ����
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



    // ���� �޴��� �̵�
    private void ReturnToMainMenu()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }

        // 0.3�� �� �� ��ȯ (�Ҹ� �鸰 �� �̵�)
        Invoke("LoadMainMenu", 0.3f);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("StartMenuScene");
    }
}
