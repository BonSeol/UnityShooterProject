using UnityEngine;

public class CardB3Item : MonoBehaviour
{
    public GameObject EKeyUI_0;
    public bool playerInRange;
    public Gamemanager Gmscript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            Gmscript.CardB3Get = 1;
            Debug.Log("지하 2층 카드 휙득");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;

            Debug.Log("컨트롤 아이템 범위 안");
            EKeyUI_0.SetActive(true); //E 버튼 가이드 UI
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;

            Debug.Log("컨트롤 아이템 범위 밖");



            EKeyUI_0.SetActive(false); //E 버튼 가이드 UI
        }
    }

    //public void ShowButton()
    //{

    //animator.SetBool("Appear",true);
    //}
}


