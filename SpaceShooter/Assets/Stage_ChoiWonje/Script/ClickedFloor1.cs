using UnityEngine;

public class ClickedFloor1 : MonoBehaviour
{
    private static ClickedFloor1 instance;

    public ElevatorLevorUI ElevatorLevorUI;
    public ElevatorCountUI ElevatorCountUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JustClicked()
    {

        Debug.Log("1F Clicked");
        ElevatorLevorUI.floor = 0;
        ElevatorCountUI.floor = 0;
    }
}
