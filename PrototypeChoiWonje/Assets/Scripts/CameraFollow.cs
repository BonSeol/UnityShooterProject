using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Func<Vector3> GetcameraFollowPositionFunc;
    
    public void Setup(Func<Vector3> GetcameraFollowPositionFunc)
    {
        this.GetcameraFollowPositionFunc = GetcameraFollowPositionFunc;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraFollowPosition = GetcameraFollowPositionFunc();
        cameraFollowPosition.z = transform.position.z;

        Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowPosition, transform.position);
        float cameraMoveSpeed = 8f;

        transform.position = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;
    }
}
