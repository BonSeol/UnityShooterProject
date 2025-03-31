using UnityEngine;

public class ElevatorLevorUI : MonoBehaviour
{
    public int floor=0;
    public GameObject Player;
    public Gamemanager Gmrscript;
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
        Debug.Log("Levor Clicked");
        
        Debug.Log(floor);
        if (floor==0)
        { 
            Player.transform.position = new Vector3(76.4688f, 8.686f, 0);
        }
        else if (floor == 1 && Gmrscript.CardB1Get == 1)
        {
            Player.transform.position = new Vector3(74.465f, 103.675f, 0);
        }
        else if (floor == 2 && Gmrscript.CardB2Get == 1)
        {
            Player.transform.position = new Vector3(74.465f, 163.686f, 0);
        }
        else if (floor == 3 && Gmrscript.CardB3Get == 1)
        {
            Player.transform.position = new Vector3(74.465f, 270.686f, 0);
        }
    }
}
