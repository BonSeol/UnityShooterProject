using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public int maxHealth = 10; // 최대 체력
    public int currentHealth; // 현재 체력
    public GameObject heartPrefab; // 하트 프리팹
    public Transform heartContainer; // 하트 UI가 위치할 부모 오브젝트
    public GameObject GameOverUI; // 게임 오버 UI
    public GameObject ClearUI; // 클리어 UI

    private List<GameObject> hearts = new List<GameObject>();

    void Start()
    {
        GameOverUI.SetActive(false);
        currentHealth = 5;
        UpdateHearts();
    }

    void UpdateHearts()
    {
        // 기존 하트 삭제
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        // 새로운 하트 추가
        for (int i = 0; i < currentHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(heart);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        UpdateHearts();
        if (currentHealth <= 0)
        {
            GameOverUI.SetActive(true);
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHearts();
    }
}
