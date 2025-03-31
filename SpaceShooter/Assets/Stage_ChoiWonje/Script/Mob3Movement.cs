using UnityEngine;

public class Mob3Movement : MonoBehaviour
{
    public ControlWall ControlWallscript;
    // 클릭 상태를 추적하는 변수
    public bool isActive = false;

    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    // 활성화된 상태의 스프라이트, 비활성화된 상태의 스프라이트
    public Sprite activeSprite;
    public Sprite inactiveSprite;


    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public float searchDistance;
    public int hp;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject projectile;
    private Transform player;
    // Start is called before the first frame update
    Animator animator;
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlWallscript.isActive == false)
        {
            animator.Play("Mob3_WakeUp");
            if (Vector2.Distance(transform.position, player.position) < searchDistance) // searchdistance 안에 플레이어가 있다면
            {
                if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                }
                else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
                {
                    transform.position = this.transform.position;
                }
                else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -speed * Time.deltaTime);
                }

                if (timeBtwShots <= 0 && Vector2.Distance(transform.position, player.position) < stoppingDistance)
                {
                    Instantiate(projectile, transform.position, Quaternion.identity);
                    timeBtwShots = startTimeBtwShots;
                    Destroy(gameObject);
                }
                else
                {
                    timeBtwShots -= Time.deltaTime;
                }
            }


        }
        else if (ControlWallscript.isActive == false)
        {
            animator.Play("Mob3_Sleep");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Bullet"))
        {
            --hp;
            if (hp <= 0)
                Destroy(gameObject);
        }

    }
}
