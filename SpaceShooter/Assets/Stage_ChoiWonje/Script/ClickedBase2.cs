using UnityEngine;

public class ClickedBase2 : MonoBehaviour
{
    private static ClickedBase2 instance;

    public ElevatorLevorUI ElevatorLevorUI;
    public ElevatorCountUI ElevatorCountUI;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void JustClicked()
    {
        ElevatorLevorUI.floor = 2;
        ElevatorCountUI.floor = 2;
    }
}
