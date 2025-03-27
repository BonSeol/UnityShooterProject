using UnityEngine.Tilemaps;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public Tilemap monsterSpawnTilemap; // ���� ���� Ÿ�ϸ�
    public GameObject[] monsterPrefabs; // ���� �����յ�
    public bool hasSpawned = false; // ���Ͱ� �� ���� �����ǵ��� ����
}