using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public float rangeX = 2f; // x ���� ����
    public float StartTime = 0.1f; // ����
    public float SpawnStop = 10f;    // ���� ������ �ð�
    public GameObject monster;

    bool isSpawning = false; // ���� ����

    bool swi = true;

    void Start()
    {
        //StartCoroutine("RandomSpawn");
        Invoke("Stop", SpawnStop);
    }

    // �ܺο��� ȣ���� �� �ִ� StartSpawning �޼��� �߰�
    public void StartSpawning()
    {
        if (!isSpawning) // �̹� ������ ���۵��� �ʾҴٸ�
        {
            swi = true;
            isSpawning = true; // ���� ���·� ����
            StartCoroutine("RandomSpawn"); // ���� ���� �ڷ�ƾ ����
            Invoke("Stop", SpawnStop);
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

            // x�� ����
            float x = Random.Range(transform.position.x - rangeX, transform.position.x + rangeX);

            // x���� ����, y���� �ڱ��ڽ� ��
            Vector2 r = new Vector2(x, transform.position.y);

            // ���� ����
            Instantiate(monster, r, Quaternion.identity);
        }
    }

    void Stop()
    {
        swi = false;
        StopCoroutine("RandomSpawn");
    }

    void Update()
    {

    }
}
