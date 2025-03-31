using UnityEngine;

public class Boss1Movement : MonoBehaviour
{
    // 클릭 상태를 추적하는 변수
    public bool isActive = false;

    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    // 활성화된 상태의 스프라이트, 비활성화된 상태의 스프라이트
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    public GameObject ClearUI; // 클리어 UI

    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public float searchDistance;
    public int hp;

    private float timeBtwShots1;
    private float timeBtwShots2;
    private float timeBtwShots3;
    public float startTimeBtwShots1;
    public float startTimeBtwShots2;
    public float startTimeBtwShots3;

    public GameObject projectile1;
    public GameObject projectile2;
    public GameObject projectile3;
    private Transform player;
    public Transform bul1;
    public Transform bul2;
    public Transform bul3;
    public Transform bul4;
    public Transform mob1;
    public Transform mob2;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        ClearUI.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots1 = startTimeBtwShots1;
        timeBtwShots2 = startTimeBtwShots2;
        timeBtwShots3 = startTimeBtwShots3;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector2.Distance(transform.position, player.position) < searchDistance) // searchdistance 안에 플레이어가 있다면
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
            {
                //animator.Play("Boss1_Walk");
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
            {
               // animator.Play("Boss1_Walk");
                transform.position = this.transform.position;
            }
            else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
            {
               // animator.Play("Boss1_Walk");
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -speed * Time.deltaTime);
            }

            if (timeBtwShots1 <= 0)
            {
                Instantiate(projectile1, bul1.position, Quaternion.identity);
                Instantiate(projectile1, bul2.position, Quaternion.identity);
                Instantiate(projectile1, bul3.position, Quaternion.identity);
                Instantiate(projectile1, bul4.position, Quaternion.identity);
                timeBtwShots1 = startTimeBtwShots1;

            }
            if (timeBtwShots2 <= 0)
            {
                Instantiate(projectile2, mob1.position, Quaternion.identity);
                Instantiate(projectile3, mob1.position, Quaternion.identity);
                timeBtwShots2 = startTimeBtwShots2;

            }
            if (timeBtwShots3 <= 0)
            {
                Instantiate(projectile2, mob2.position, Quaternion.identity);
                Instantiate(projectile3, mob2.position, Quaternion.identity);
                timeBtwShots3 = startTimeBtwShots3;

            }
            else
            {
                timeBtwShots1 -= Time.deltaTime;
                timeBtwShots2 -= Time.deltaTime;
                timeBtwShots3 -= Time.deltaTime;
            }
        }
            
           
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Bullet"))
        {
            --hp;
            if (hp==0)
            {
                animator.SetTrigger("Death");
                ClearUI.SetActive(true);
            }
               
        }
        
    }

}
