using UnityEngine;

public class Mob1Projectile : MonoBehaviour
{
    public float speed;
    
    private Transform player;
    private Vector2 target;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector2(player.position.x, player.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();
        }
           
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //rb.linearVelocity = Vector2.zero;
            Player player = other.GetComponent<Player>();


            if (player != null)
                player.TakeDamage(1); // 플레이어 데미지 줌
            DestroyProjectile();
        }
        if (other.CompareTag("Wall"))
        {
            //animator.Play("Hit");
           // rb.linearVelocity = Vector2.zero;
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        // Destroy(gameObject, 0.2f);// 애니메이션 끝난 후 삭제
        Destroy(gameObject);
    }
}
