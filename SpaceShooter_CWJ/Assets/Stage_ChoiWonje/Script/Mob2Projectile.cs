using UnityEngine;
using System.Collections;

public class Mob2Projectile : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector2 target;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(waiter());
          
    }
    IEnumerator waiter()
    {
        
        //��ٸ�
        yield return new WaitForSecondsRealtime(1);
        DestroyProjectile();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.Find("Player").GetComponent<Player>().TakeDamage(1);
            
        }
        if (other.CompareTag("Wall"))
        {
            
            
        }
    }
    void DestroyProjectile()
    {
        // Destroy(gameObject, 0.2f);// �ִϸ��̼� ���� �� ����
        Destroy(gameObject);
    }
}
