using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockPath1 : MonoBehaviour
{
    public Tilemap tilemap;  // 타일맵 연결
    public Tile wallTile;    // 벽 타일
    public Tile openTile;    // 원래 길 타일
    public Spawn Bat1; 
    public Spawn Bat2; 
    public Spawn Sk1; 
    public BossSpawn bossSpawn; // 보스 스폰 관리 클래스

    private bool isBlocked = false;  // 길이 막혀 있는지 여부
    private BoxCollider2D col;  // Blocker의 Collider

    private void Start()
    {
        if (tilemap == null)
            tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>(); // Tilemap 자동 찾기

        col = GetComponent<BoxCollider2D>(); // Collider 가져오기     
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어가 닿았을 때
        {
            Vector3Int cellPos = tilemap.WorldToCell(transform.position); // 현재 Blocker 위치의 타일 좌표 가져오기

            if (isBlocked == false) // 첫 번째로 닿았을 때 (길을 열기)
            {
                tilemap.SetTile(cellPos, openTile); // 원래 길 타일로 변경
                col.isTrigger = true;
                isBlocked = true; // 길을 열었으니 이제 두 번째로 막히게 설정

                Bat1.StartSpawning();
                Bat2.StartSpawning();
                Sk1.StartSpawning();

                Invoke("SpawnBoss", 10f); // 10초 후에 보스 스폰
            }
            else
            {
                tilemap.SetTile(cellPos, wallTile); // 벽 타일로 변경
                col.isTrigger = false;
            }
        }
    }
    private void SpawnBoss()
    {
        bossSpawn.SpawnBoss(); // 보스 스폰 클래스에서 소환
    }
}
