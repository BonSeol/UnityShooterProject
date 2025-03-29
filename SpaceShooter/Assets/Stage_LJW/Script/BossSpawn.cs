using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject bossPrefab; // ���� ������
    public Transform spawnPoint;  // ������ ��ȯ�� ��ġ
    public Transform[] waypoints; // ������ �̵� ��� (�� �� ������Ʈ)

    private bool hasSpawned = false; // ������ ��ȯ�Ǿ����� üũ

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
