using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public GameObject firePos;
    public AudioClip shootSound;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        AimAtMouse();

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void AimAtMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.z * -1f;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 direction = worldMousePos - transform.position;
        direction.z = 0f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePos.transform.position, transform.rotation);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.z * -1f;
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector2 fireDirection = (worldMousePos - firePos.transform.position).normalized;

            bulletRb.linearVelocity = fireDirection * bulletSpeed;
        }

        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
