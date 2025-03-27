using UnityEngine.Tilemaps;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public Tilemap monsterSpawnTilemap; // 몬스터 스폰 타일맵
    public GameObject[] monsterPrefabs; // 몬스터 프리팹들
    public bool hasSpawned = false; // 몬스터가 한 번만 생성되도록 설정
}