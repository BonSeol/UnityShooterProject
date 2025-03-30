using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    [Header("필수 설정")]
    public GameObject lazerPrefab;          // 애니메이션 달린 레이저 프리팹 (한 칸짜리)
    [HideInInspector] public Transform[] spawnPoints; // 외부에서 넘겨받는 레이저 위치

    public float lazerDuration = 1f;

    private Coroutine lazerRoutine;
    private List<GameObject> lazers = new List<GameObject>();

    public void StartLazer()
    {
        if (lazerRoutine != null)
            return;

        if (lazerPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            return;
        }

        // spawnPoints 위치에 실제 레이저 프리팹을 생성
        foreach (var point in spawnPoints)
        {
            GameObject l = Instantiate(lazerPrefab, point.position, Quaternion.identity);
            l.SetActive(false);
            lazers.Add(l);
        }

        lazerRoutine = StartCoroutine(CycleLazer());
    }

    IEnumerator CycleLazer()
    {
        int index = 0;

        while (true)
        {
            // 모든 레이저 끄기
            foreach (var l in lazers)
                l.SetActive(false);

            // 현재 레이저 하나만 켜기
            lazers[index].SetActive(true);

            yield return new WaitForSeconds(lazerDuration);

            index = (index + 1) % lazers.Count;
        }
    }

    public void StopLazer()
    {
        if (lazerRoutine != null)
        {
            StopCoroutine(lazerRoutine);
            lazerRoutine = null;
        }

        foreach (var l in lazers)
        {
            if (l != null)
                l.SetActive(false);
        }

    }
}
