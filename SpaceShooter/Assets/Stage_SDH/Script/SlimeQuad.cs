using UnityEngine;

public class SlimeQuad : Monster
{
    // 애니메이션 이벤트에서 호출될 함수
    protected override void ShootBullet()
    {
        if (isDead) return;

        audioSource.PlayOneShot(shootSound);

        float[] angles = { 45f, 135f, 225f, 315f};

        foreach (float offset in angles)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            Vector2 shootDirection = (player.position - firePoint.position).normalized;
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg + offset;
            Vector2 rotatedDirection = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ).normalized;

            bulletRb.linearVelocity = rotatedDirection * bulletSpeed;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        FlipSprite((player.position - firePoint.position).x);
    }
}
