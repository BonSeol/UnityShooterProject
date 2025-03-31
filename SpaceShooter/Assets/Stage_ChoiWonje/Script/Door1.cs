using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Door1 : MonoBehaviour
{
    
    public int open = 0;
    Animator animator;
    GameObject Player;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
    }
    

    void OnTriggerEnter2D(Collider2D collisionOpen)
    {
       // Debug.Log("문이 충돌받음");
        if (collisionOpen.gameObject.tag == "Player")
        {
            
            animator.Play("Door1_Open");
            
        }
        
    }
    void OnTriggerStay2D(Collider2D collisionOpen)
    {
        
        

    }

    void OnTriggerExit2D(Collider2D collisionOpen)
    {
        //Debug.Log("문이 충돌받음");
        if (collisionOpen.gameObject.tag == "Player")
        {
            
                animator.Play("Door1_Close");
                

        }

    }

    void OnCollisionStay2D(Collision2D collisionOpen)
    {
        if (collisionOpen.gameObject.tag == "Player")
        {
            Player.transform.position = new Vector3(76.52f, 5.56f, 0);
            Debug.Log("워프");
            animator.Play("Door1_Open_While");

        }
        
    }


    void Update()
    {
       
       // Debug.Log("문 상태 업데이트");
        
    }

    
}
