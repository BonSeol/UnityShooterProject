using UnityEngine;

public class CardB3Item : MonoBehaviour
{
    public GameObject EKeyUI_0;
    public bool playerInRange;
    public Gamemanager Gmscript;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            Gmscript.CardB3Get = 1;
            Debug.Log("���� 2�� ī�� �׵�");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;

            Debug.Log("��Ʈ�� ������ ���� ��");
            EKeyUI_0.SetActive(true); //E ��ư ���̵� UI
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;

            Debug.Log("��Ʈ�� ������ ���� ��");



            EKeyUI_0.SetActive(false); //E ��ư ���̵� UI
        }
    }

}


