using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class FootPrintDoor : MonoBehaviour
{
    public GameObject ColDoor;
    public int open = 0;
    Animator animator;
    public Sprite open_image;
    void Start()
    {

       // animator = GetComponent<Animator>();
    }
    

    void OnTriggerEnter2D(Collider2D collisionOpen)
    {
      
        if (collisionOpen.gameObject.tag == "Player")
        {
            open = 1;
            //animator.Play("Door1_Open");
            gameObject.GetComponent<SpriteRenderer>().sprite = open_image;
            
        }
        
    }
    
    
   

    void Update()
    {
        if(open == 0)
        {
            ColDoor.GetComponent<BoxCollider2D>().enabled = true;
            
        }
        else if (open == 1)
        {
            ColDoor.GetComponent<BoxCollider2D>().enabled = false;
            
        }
    }

    
}
