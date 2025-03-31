using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ElevatorCountUI : MonoBehaviour
{
    public int floor = 0;

    public Sprite base3; 
    public Sprite base2; 
    public Sprite base1;
    public Sprite floor1;
    public Sprite Nocard;
    public Gamemanager Gmscript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (floor==0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = floor1;
        }
        else if (floor == 1)
        {
            if(Gmscript.CardB1Get == 1)
            this.gameObject.GetComponent<SpriteRenderer>().sprite = base1;
            else
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Nocard;
        }
        else if (floor == 2)
        {
            if (Gmscript.CardB2Get == 1)
                this.gameObject.GetComponent<SpriteRenderer>().sprite = base2;
            else
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Nocard;
        }
        else if (floor == 3)
        {
            if (Gmscript.CardB3Get == 1)
                this.gameObject.GetComponent<SpriteRenderer>().sprite = base3;
            else
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Nocard;
        }

    }
}
