using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public int maxHealth = 10; // �ִ� ü��
    public int currentHealth; // ���� ü��
    public GameObject heartPrefab; // ��Ʈ ������
    public Transform heartContainer; // ��Ʈ UI�� ��ġ�� �θ� ������Ʈ

    private List<GameObject> hearts = new List<GameObject>();

    void Start()
    {
        currentHealth = 5;
        UpdateHearts();
    }

    void UpdateHearts()
    {
        // ���� ��Ʈ ����
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        // ���ο� ��Ʈ �߰�
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
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHearts();
    }
}
