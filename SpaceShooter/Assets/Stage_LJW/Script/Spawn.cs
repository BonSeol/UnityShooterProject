using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public float rangeX = 2f; // x 범위 조정
    public float StartTime = 0.1f; // 시작
    public float SpawnStop = 10f;    // 스폰 끝나는 시간
    public GameObject monster;

    bool isSpawning = false; // 스폰 여부

    bool swi = true;

    void Start()
    {
        //StartCoroutine("RandomSpawn");
        Invoke("Stop", SpawnStop);
    }

    // 외부에서 호출할 수 있는 StartSpawning 메서드 추가
    public void StartSpawning()
    {
        if (!isSpawning) // 이미 스폰이 시작되지 않았다면
        {
            swi = true;
            isSpawning = true; // 스폰 상태로 설정
            StartCoroutine("RandomSpawn"); // 랜덤 스폰 코루틴 시작
            Invoke("Stop", SpawnStop);
        }
    }

    // 코루틴으로 랜덤하게 생성하기
    IEnumerator RandomSpawn()
    {
        isSpawning = true; // 스폰 시작

        while (swi)
        {
            // 1초 마다
            yield return new WaitForSeconds(StartTime);

            // x값 랜덤
            float x = Random.Range(transform.position.x - rangeX, transform.position.x + rangeX);

            // x값은 랜덤, y값은 자기자신 값
            Vector2 r = new Vector2(x, transform.position.y);

            // 몬스터 생성
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
