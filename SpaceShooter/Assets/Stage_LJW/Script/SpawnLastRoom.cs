using System.Collections;
using UnityEngine;

public class SpawnLastRoom : MonoBehaviour
{
    public float rangeY = 2f; // y 범위 조정
    public float StartTime = 0.1f; // 시작
    public float SpawnStop = 10f;    // 스폰 끝나는 시간
    public GameObject monster;
    private Coroutine spawnRoutine;

    bool isSpawning = false; // 스폰 여부

    bool swi = true;

    void Start()
    {

    }

    // 외부에서 호출할 수 있는 StartSpawning 메서드 추가
    public void StartSpawning()
    {
        if (!isSpawning) // 이미 스폰이 시작되지 않았다면
        {
            swi = true;
            isSpawning = true; // 스폰 상태로 설정
            spawnRoutine = StartCoroutine(RandomSpawn()); // 랜덤 스폰 코루틴 시작

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

            // y값 랜덤
            float y = Random.Range(transform.position.y - rangeY, transform.position.y + rangeY);

            // y값은 랜덤, x값은 자기자신 값
            Vector2 r = new Vector2(transform.position.x, y);

            // 몬스터 생성
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
