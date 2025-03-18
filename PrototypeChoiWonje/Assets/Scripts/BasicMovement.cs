using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicMovement : MonoBehaviour
{
    public Animator animator;
    public GameObject crossHair;

   // private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));//

        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        //transform.position = transform.position + horizontal * (Time.deltaTime);
        transform.position = transform.position + horizontal * (Time.deltaTime) * 5;
    }

    /*private void MoveCrossHair()
    {
        Vector3 aim = new Vector3(player.GetAxis("AimHorizontal"), player.GetAxis("AimVertical"), 0.0f);

        if(aim.magnitude > 0.0f)
        {
            aim.Normalize();
            crossHair.transform.localPosition = aim;
        }

    }*/
}

