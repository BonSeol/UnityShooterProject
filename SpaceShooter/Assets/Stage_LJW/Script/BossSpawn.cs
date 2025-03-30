using UnityEngine;
using UnityEngine.Tilemaps;

public class BossSpawn : MonoBehaviour
{
    public GameObject bossPrefab; // ���� ������
    public Transform spawnPoint;  // ������ ��ȯ�� ��ġ
    public Transform[] waypoints; // ������ �̵� ��� (�� �� ������Ʈ)

    private bool hasSpawned = false; // ������ ��ȯ�Ǿ����� üũ

    private Lazer lazerInstance;

    [Header("Boss.cs")]
    public SpawnLastRoom[] spawners;        // ���� ������
    public Tilemap tilemap;                 // Ÿ�ϸ�
    public Tile stairTile;                  // ��� Ÿ��
    public Transform stairWorldPos; // ����� ���� ���� ���� ��ǥ ��ġ
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
