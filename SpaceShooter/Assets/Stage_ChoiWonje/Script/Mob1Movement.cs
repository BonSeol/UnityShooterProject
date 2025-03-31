using UnityEngine;

public class Mob1Movement : MonoBehaviour
{
    // Ŭ�� ���¸� �����ϴ� ����
    public bool isActive = false;

    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    // Ȱ��ȭ�� ������ ��������Ʈ, ��Ȱ��ȭ�� ������ ��������Ʈ
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
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) < searchDistance) // searchdistance �ȿ� �÷��̾ �ִٸ�
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

            if (timeBtwShots <= 0)
            {
                Instantiate(projectile, transform.position, Quaternion.identity);
                timeBtwShots = startTimeBtwShots;

            }
            else
            {
                timeBtwShots -= Time.deltaTime;
            }
        }
            
           
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Bullet"))
        {
            --hp;
            if (hp<=0)
                Destroy(gameObject);
        }
        
    }
}
