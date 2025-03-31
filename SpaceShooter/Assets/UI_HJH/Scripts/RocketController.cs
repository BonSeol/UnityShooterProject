using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketController : MonoBehaviour
{
    private Animator animator;
    public GameObject player;


    void Start()
    {
        animator = GetComponent<Animator>();
        player.SetActive(false);

        // 시작 시 착륙 후 플레이어 활성화, 이후 상승
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        animator.SetTrigger("Land");
        yield return new WaitForSeconds(3f); // 애니메이션 길이에 맞게 설정
        player.SetActive(true);
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("TakeOff");
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

}
