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

        // ���� �� ���� �� �÷��̾� Ȱ��ȭ, ���� ���
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        animator.SetTrigger("Land");
        yield return new WaitForSeconds(3f); // �ִϸ��̼� ���̿� �°� ����
        player.SetActive(true);
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("TakeOff");
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

}
