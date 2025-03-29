using UnityEngine;

public class ClickedBase2 : MonoBehaviour
{
    private static ClickedBase2 instance;

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
        ElevatorLevorUI.floor = 2;
        ElevatorCountUI.floor = 2;
    }
}
