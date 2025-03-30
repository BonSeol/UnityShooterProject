using System.Collections;
using UnityEngine;

public class SpawnLastRoom : MonoBehaviour
{
    public float rangeY = 2f; // y ���� ����
    public float StartTime = 0.1f; // ����
    public float SpawnStop = 10f;    // ���� ������ �ð�
    public GameObject monster;
    private Coroutine spawnRoutine;

    bool isSpawning = false; // ���� ����

    bool swi = true;

    void Start()
    {

    }

    // �ܺο��� ȣ���� �� �ִ� StartSpawning �޼��� �߰�
    public void StartSpawning()
    {
        if (!isSpawning) // �̹� ������ ���۵��� �ʾҴٸ�
        {
            swi = true;
            isSpawning = true; // ���� ���·� ����
            spawnRoutine = StartCoroutine(RandomSpawn()); // ���� ���� �ڷ�ƾ ����

        }
    }

    // �ڷ�ƾ���� �����ϰ� �����ϱ�
    IEnumerator RandomSpawn()
    {
        isSpawning = true; // ���� ����

        while (swi)
        {
            // 1�� ����
            yield return new WaitForSeconds(StartTime);

            // y�� ����
            float y = Random.Range(transform.position.y - rangeY, transform.position.y + rangeY);

            // y���� ����, x���� �ڱ��ڽ� ��
            Vector2 r = new Vector2(transform.position.x, y);

            // ���� ����
            Instantiate(monster, r, Quaternion.identity);
        }

        isSpawning = false;
    }

    public void StopSpawning()
    {

        swi = false;
        isSpawning = false;

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
       
    }

    void Update()
    {

    }
}
