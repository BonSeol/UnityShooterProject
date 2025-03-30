using UnityEngine;
using UnityEngine.Tilemaps;

public class BossSpawn : MonoBehaviour
{
    public GameObject bossPrefab; // 보스 프리팹
    public Transform spawnPoint;  // 보스를 소환할 위치
    public Transform[] waypoints; // 보스의 이동 경로 (씬 내 오브젝트)

    private bool hasSpawned = false; // 보스가 소환되었는지 체크

    private Lazer lazerInstance;

    [Header("Boss.cs")]
    public SpawnLastRoom[] spawners;        // 몬스터 스폰들
    public Tilemap tilemap;                 // 타일맵
    public Tile stairTile;                  // 계단 타일
    public Transform stairWorldPos; // 계단이 나올 실제 월드 좌표 위치
    public Transform holePos1;
    public Transform holePos2;
    public Transform holePos3;

    public void SpawnBoss()
    {
        if (!hasSpawned && bossPrefab != null && spawnPoint != null)
        {
            GameObject bossInstance = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
            Boss bossScript = bossInstance.GetComponent<Boss>();
            

            if (bossScript != null)
            {
                bossScript.SetWaypoints(waypoints);
                bossScript.SetHolePositions(holePos1, holePos2, holePos3);
                bossScript.onDeath += OnBossDefeated;
            }
           
            hasSpawned = true;
        }
    }

    public void SetLazerInstance(Lazer lazer)
    {
        lazerInstance = lazer;
    }

    private void OnBossDefeated()
    {

        if (lazerInstance != null)
        {
            lazerInstance.StopLazer();
        }

        foreach (var s in spawners)
        {
            if (s == null)
            {
                continue;
            }
            s.StopSpawning();
        }

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (var monster in monsters)
        {
            Destroy(monster);
        }

        Vector3Int cellPos = tilemap.WorldToCell(stairWorldPos.position);
        tilemap.SetTile(cellPos, stairTile);

    }

}
