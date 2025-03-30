using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Door2 : MonoBehaviour
{
    
    
    GameObject Player;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
      
    }

    void OnCollisionStay2D(Collision2D collisionOpen)
    {
        if (collisionOpen.gameObject.tag == "Player")
        {
            Player.transform.position = new Vector3(-0.518f, 4.46f, 0);
        }
        
    }


    void Update()
    {
       
      
        
    }

    
}
