using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // �̱��� �ν��Ͻ�

    public int gemCount = 0; // ���� ���� üũ
    public int AltarCount = 0; // ��� ���� üũ

    void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // �ߺ��� GameManager�� �����Ǹ� ����
            return;
        }

        DontDestroyOnLoad(gameObject); // ���� ����Ǿ GameManager ����
    }
}
