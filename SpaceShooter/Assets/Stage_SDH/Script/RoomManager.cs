using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public RoomData room; // 각 방에 대한 데이터
    public GameObject roomLightParent; // 현재 방의 조명이 모여 있는 빈 오브젝트
    private Collider2D entranceCollider; // 방 입구의 콜라이더를 담을 변수

    static List<RoomManager> allRooms = new List<RoomManager>(); // 모든 방 목록
    private List<GameObject> roomLights = new List<GameObject>(); // 현재 방의 조명 리스트

    void Awake()
    {
        // 모든 RoomManager를 리스트에 추가
        if (!allRooms.Contains(this))
        {
            allRooms.Add(this);
        }
    }

    void Start()
    {
        // 방 입구의 콜라이더 가져오기 (RoomManager는 입구 오브젝트에 붙어 있어야 함)
        entranceCollider = GetComponent<Collider2D>();

        // roomLightParent의 모든 자식 오브젝트를 roomLights 리스트에 추가
        if (roomLightParent != null)
        {
            foreach (Transform child in roomLightParent.transform)
            {
                roomLights.Add(child.gameObject);
                child.gameObject.SetActive(false); // 시작할 때 모든 조명 끄기
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 방 입구를 지나면 조명을 켜고, 몬스터를 스폰
        if (other.CompareTag("Player"))
        {
            ActivateRoomLights(); // 현재 방 조명 켜기
            if (!room.hasSpawned)
            {
                room.hasSpawned = true;
                StartCoroutine(SpawnMonstersWithDelay()); // 0.1초 뒤 몬스터 스폰
            }
        }
    }

    void ActivateRoomLights()
    {
        // 모든 방의 조명을 끄기
        foreach (var room in allRooms)
        {
            if (room != null)
            {
                room.DeactivateLights();
            }
        }

        // 현재 방의 조명만 켜기
        foreach (var light in roomLights)
        {
            if (light != null)
            {
                light.SetActive(true);
            }
        }
    }

    void DeactivateLights()
    {
        // 이 방의 조명을 끄기
        foreach (var light in roomLights)
        {
            if (light != null)
            {
                light.SetActive(false);
            }
        }
    }

    IEnumerator SpawnMonstersWithDelay()
    {
        yield return new WaitForSeconds(0.1f); // 0.1초 대기
        SpawnMonsters(); // 몬스터 스폰 실행
    }

    void SpawnMonsters()
    {
        BoundsInt bounds = room.monsterSpawnTilemap.cellBounds;  // 타일맵의 범위

        // bounds로 타일 범위 설정
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);  // 현재 셀의 그리드 좌표

                if (room.monsterSpawnTilemap.HasTile(tilePos))  // 해당 위치에 타일이 있으면
                {
                    // 타일의 월드 좌표를 구해서 몬스터 스폰
                    Vector3 worldPos = room.monsterSpawnTilemap.GetCellCenterWorld(tilePos);
                    GameObject monsterPrefab = room.monsterPrefabs[Random.Range(0, room.monsterPrefabs.Length)];
                    Instantiate(monsterPrefab, worldPos, Quaternion.identity);  // 몬스터 스폰
                }
            }
        }
    }
}
