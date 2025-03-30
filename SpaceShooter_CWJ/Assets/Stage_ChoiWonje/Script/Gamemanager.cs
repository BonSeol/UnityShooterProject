using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public int CardB3Get;
    public int CardB2Get;
    public int CardB1Get;
    public int ControlitemGet;
    public GameObject Playerobject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        Playerobject.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
