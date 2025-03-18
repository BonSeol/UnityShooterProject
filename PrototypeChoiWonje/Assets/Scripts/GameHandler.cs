using System.Xml.Serialization;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CameraFollow cameraFollow;
    public Transform playerTransform;
    private void Start()
    {
        cameraFollow.Setup(() => playerTransform.position);
    }
}
