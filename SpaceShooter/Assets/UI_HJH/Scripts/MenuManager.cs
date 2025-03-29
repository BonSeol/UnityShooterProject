using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public GameObject menuUI;  // ESC�� ���ݴ� �޴� UI
    public Transform itemPanel; // ������ ǥ���� �г� (��ĭ)
    public Transform weaponPanel; // ���� ǥ���� �г� (���� �Ʒ�)
    public Text stageText;  // ���� �������� ǥ���� �ؽ�Ʈ
    public Button exitButton;  // EXIT ��ư

    public AudioSource audioSource; // ����� �ҽ�
    public AudioClip menuOpenSound; // �޴� ���� �� �Ҹ�
    public AudioClip buttonClickSound; // ��ư Ŭ�� �Ҹ�

    public GameObject itemSlotPrefab; // ������ ���� ������
    public GameObject weaponSlotPrefab; // ���� ���� ������

    private List<GameObject> currentItems = new List<GameObject>();
    private List<GameObject> currentWeapons = new List<GameObject>();
    private int currentStage = 1; // ���� ��������

    void Start()
    {
        menuUI.SetActive(false); // ������ �� �޴� ����
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

    // ������ �߰� �Լ�
    public void AddItem(Sprite itemSprite)
    {
        GameObject newItem = Instantiate(itemSlotPrefab, itemPanel);
        newItem.GetComponent<Image>().sprite = itemSprite;
        currentItems.Add(newItem);
    }

    // ���� ���� �Լ�
    public void SetWeapons(Sprite weapon1, Sprite weapon2)
    {
        // ���� ���� ����
        foreach (GameObject weapon in currentWeapons)
        {
            Destroy(weapon);
        }
        currentWeapons.Clear();

        // ���ο� ���� �߰�
        GameObject weaponSlot1 = Instantiate(weaponSlotPrefab, weaponPanel);
        weaponSlot1.GetComponent<Image>().sprite = weapon1;
        currentWeapons.Add(weaponSlot1);

        GameObject weaponSlot2 = Instantiate(weaponSlotPrefab, weaponPanel);
        weaponSlot2.GetComponent<Image>().sprite = weapon2;
        currentWeapons.Add(weaponSlot2);
    }

    // ���� �������� ������Ʈ
    public void SetStage(int stage)
    {
        currentStage = stage;
        UpdateStageUI();
    }

    private void UpdateStageUI()
    {
        stageText.text = "Stage " + currentStage;
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
