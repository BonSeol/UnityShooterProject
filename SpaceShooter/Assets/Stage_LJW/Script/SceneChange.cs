using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string nextSceneName;  // �̵��� �� �̸�

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // �÷��̾ ��Ҵ��� Ȯ��
        {
            SceneManager.LoadScene(nextSceneName);  // �� ��ȯ
        }
    }
}
