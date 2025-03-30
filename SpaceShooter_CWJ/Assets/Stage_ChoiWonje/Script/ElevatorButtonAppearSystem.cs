using UnityEngine;

public class ElevatorButtonUISystem : MonoBehaviour
{
    private static ElevatorButtonUISystem instance;

    public Animator animator;
    public GameObject ElevatorButtonUI_0;
    public GameObject EKeyUI_0;
    public Gun Gun1;
    public Gun Gun2;
    public bool printUI = false;
    public bool playerInRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

  
    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            if(printUI == true)
            {
                //ElevatorButtonUI_0.SetActive(false);
                Debug.Log("���������� ��ư UI �����");
                animator.SetBool("Appear",false);
                printUI = false;
                //Cursor.visible = false;
                Gun1.player_interaction = false;
                Gun2.player_interaction = false;
            }
            else if(printUI == false)
            {
                //ElevatorButtonUI_0.SetActive(true);
                Debug.Log("���������� ��ư UI ����");
                animator.SetBool("Appear", true);
                printUI = true;
                //Cursor.visible = true;
                Gun1.player_interaction = true;
                Gun2.player_interaction = true;
            }

            Debug.Log("���������� ��ư ��ȣ�ۿ� �Է�");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            playerInRange = true;
            ElevatorButtonUI_0.SetActive(true);
            Debug.Log("���������� ��ư ���� ��");
            EKeyUI_0.SetActive(true); //E ��ư ���̵� UI
        }
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            playerInRange = false;
            ElevatorButtonUI_0.SetActive(false);
            Debug.Log("����������  ��ư ���� ��");
            animator.SetBool("Appear", false);
            printUI = false;
            Gun1.player_interaction = false;
            Gun2.player_interaction = false;
            EKeyUI_0.SetActive(false); //E ��ư ���̵� UI
        }
    }

    //public void ShowButton()
    //{
       
        //animator.SetBool("Appear",true);
    //}
}
