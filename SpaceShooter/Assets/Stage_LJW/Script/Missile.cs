using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject target;

    public int damage = 1; // 미사일이 주는 데미지

    public float Speed = 3f;
    Vector2 dir;  
    Vector2 dirNo;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        dir = target.transform.position - transform.position;
        dirNo = dir.normalized;
    }


    void Update()
    {
        
        transform.Translate(dirNo * Speed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //collision.GetComponent<Player>().TakeDamage(1);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
