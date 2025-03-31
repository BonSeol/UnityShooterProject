using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class DoorElevator1 : MonoBehaviour
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
            Player.transform.position = new Vector3(76.49f, 33.56f, 0);
        }
        
    }

    void Update()
    {
        
    }

    
}
