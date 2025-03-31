using UnityEngine;

public class ClickedBase1 : MonoBehaviour
{
    private static ClickedBase1 instance;

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

        Debug.Log("B3F Clicked");
        ElevatorLevorUI.floor = 1;
        ElevatorCountUI.floor = 1;
    }
}
