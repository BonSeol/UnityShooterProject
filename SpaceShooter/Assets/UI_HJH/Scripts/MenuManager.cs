using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public GameObject menuUI;  // ESC로 여닫는 메뉴 UI
    public Transform itemPanel; // 아이템 표시할 패널 (윗칸)
    public Transform weaponPanel; // 무기 표시할 패널 (왼쪽 아래)
    public Text stageText;  // 현재 스테이지 표시할 텍스트
    public Button exitButton;  // EXIT 버튼

    public AudioSource audioSource; // 오디오 소스
    public AudioClip menuOpenSound; // 메뉴 열릴 때 소리
    public AudioClip buttonClickSound; // 버튼 클릭 소리

    public GameObject itemSlotPrefab; // 아이템 슬롯 프리팹
    public GameObject weaponSlotPrefab; // 무기 슬롯 프리팹

    private List<GameObject> currentItems = new List<GameObject>();
    private List<GameObject> currentWeapons = new List<GameObject>();
    private int currentStage = 1; // 현재 스테이지

    void Start()
    {
        menuUI.SetActive(false); // 시작할 때 메뉴 숨김
        exitButton.onClick.AddListener(ReturnToMainMenu);
        UpdateStageUI();
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

    // 아이템 추가 함수
    public void AddItem(Sprite itemSprite)
    {
        GameObject newItem = Instantiate(itemSlotPrefab, itemPanel);
        newItem.GetComponent<Image>().sprite = itemSprite;
        currentItems.Add(newItem);
    }

    // 무기 장착 함수
    public void SetWeapons(Sprite weapon1, Sprite weapon2)
    {
        // 기존 무기 삭제
        foreach (GameObject weapon in currentWeapons)
        {
            Destroy(weapon);
        }
        currentWeapons.Clear();

        // 새로운 무기 추가
        GameObject weaponSlot1 = Instantiate(weaponSlotPrefab, weaponPanel);
        weaponSlot1.GetComponent<Image>().sprite = weapon1;
        currentWeapons.Add(weaponSlot1);

        GameObject weaponSlot2 = Instantiate(weaponSlotPrefab, weaponPanel);
        weaponSlot2.GetComponent<Image>().sprite = weapon2;
        currentWeapons.Add(weaponSlot2);
    }

    // 현재 스테이지 업데이트
    public void SetStage(int stage)
    {
        currentStage = stage;
        UpdateStageUI();
    }

    private void UpdateStageUI()
    {
        stageText.text = "Stage " + currentStage;
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
