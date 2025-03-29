using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string nextSceneName;  // 이동할 씬 이름

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // 플레이어가 닿았는지 확인
        {
            SceneManager.LoadScene(nextSceneName);  // 씬 전환
        }
    }
}
