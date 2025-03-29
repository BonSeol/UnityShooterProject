using UnityEngine;

public class ElevatorButtonAppearSystem : MonoBehaviour
{
    private static ElevatorButtonAppearSystem instance;

    public Animator animator;
    public GameObject ElevatorButtonUI_0;
    public GameObject EKeyUI_0;
    public Gun Gun;
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
                Debug.Log("엘리베이터 버튼 UI 사라짐");
                animator.SetBool("Appear",false);
                printUI = false;

                Gun.player_interaction = false;
            }
            else if(printUI == false)
            {
                //ElevatorButtonUI_0.SetActive(true);
                Debug.Log("엘리베이터 버튼 UI 나옴");
                animator.SetBool("Appear", true);
                printUI = true;

                Gun.player_interaction = true;
            }

            Debug.Log("엘리베이터 버튼 상호작용 입력");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            playerInRange = true;
            ElevatorButtonUI_0.SetActive(true);
            Debug.Log("엘리베이터 버튼 범위 안");
            EKeyUI_0.SetActive(true); //E 버튼 가이드 UI
        }
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            playerInRange = false;
            ElevatorButtonUI_0.SetActive(false);
            Debug.Log("엘리베이터  버튼 범위 밖");
            animator.SetBool("Appear", false);
            printUI = false;
            Gun.player_interaction = false;
            EKeyUI_0.SetActive(false); //E 버튼 가이드 UI
        }
    }

    //public void ShowButton()
    //{
       
        //animator.SetBool("Appear",true);
    //}
}
