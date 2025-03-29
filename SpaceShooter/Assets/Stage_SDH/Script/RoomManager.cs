using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public RoomData room; // �� �濡 ���� ������
    public GameObject roomLightParent; // ���� ���� ������ �� �ִ� �� ������Ʈ
    private Collider2D entranceCollider; // �� �Ա��� �ݶ��̴��� ���� ����

    static List<RoomManager> allRooms = new List<RoomManager>(); // ��� �� ���
    private List<GameObject> roomLights = new List<GameObject>(); // ���� ���� ���� ����Ʈ

    void Awake()
    {
        // ��� RoomManager�� ����Ʈ�� �߰�
        if (!allRooms.Contains(this))
        {
            allRooms.Add(this);
        }
    }

    void Start()
    {
        // �� �Ա��� �ݶ��̴� �������� (RoomManager�� �Ա� ������Ʈ�� �پ� �־�� ��)
        entranceCollider = GetComponent<Collider2D>();

        // roomLightParent�� ��� �ڽ� ������Ʈ�� roomLights ����Ʈ�� �߰�
        if (roomLightParent != null)
        {
            foreach (Transform child in roomLightParent.transform)
            {
                roomLights.Add(child.gameObject);
                child.gameObject.SetActive(false); // ������ �� ��� ���� ����
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾ �� �Ա��� ������ ������ �Ѱ�, ���͸� ����
        if (other.CompareTag("Player"))
        {
            ActivateRoomLights(); // ���� �� ���� �ѱ�
            if (!room.hasSpawned)
            {
                room.hasSpawned = true;
                StartCoroutine(SpawnMonstersWithDelay()); // 0.1�� �� ���� ����
            }
        }
    }

    void ActivateRoomLights()
    {
        // ��� ���� ������ ����
        foreach (var room in allRooms)
        {
            if (room != null)
            {
                room.DeactivateLights();
            }
        }

        // ���� ���� ���� �ѱ�
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
        // �� ���� ������ ����
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
        yield return new WaitForSeconds(0.1f); // 0.1�� ���
        SpawnMonsters(); // ���� ���� ����
    }

    void SpawnMonsters()
    {
        BoundsInt bounds = room.monsterSpawnTilemap.cellBounds;  // Ÿ�ϸ��� ����

        // bounds�� Ÿ�� ���� ����
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);  // ���� ���� �׸��� ��ǥ

                if (room.monsterSpawnTilemap.HasTile(tilePos))  // �ش� ��ġ�� Ÿ���� ������
                {
                    // Ÿ���� ���� ��ǥ�� ���ؼ� ���� ����
                    Vector3 worldPos = room.monsterSpawnTilemap.GetCellCenterWorld(tilePos);
                    GameObject monsterPrefab = room.monsterPrefabs[Random.Range(0, room.monsterPrefabs.Length)];
                    Instantiate(monsterPrefab, worldPos, Quaternion.identity);  // ���� ����
                }
            }
        }
    }
}
