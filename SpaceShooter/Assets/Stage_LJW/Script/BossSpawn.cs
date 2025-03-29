using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject bossPrefab; // 보스 프리팹
    public Transform spawnPoint;  // 보스를 소환할 위치
    public Transform[] waypoints; // 보스의 이동 경로 (씬 내 오브젝트)

    private bool hasSpawned = false; // 보스가 소환되었는지 체크

    public void SpawnBoss()
    {
        if (!hasSpawned && bossPrefab != null && spawnPoint != null)
        {
            GameObject bossInstance = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
            Boss bossScript = bossInstance.GetComponent<Boss>();

            if (bossScript != null)
            {
                bossScript.SetWaypoints(waypoints); 
             
            }
            else
            {

            }

            hasSpawned = true;
        }
    }
}
