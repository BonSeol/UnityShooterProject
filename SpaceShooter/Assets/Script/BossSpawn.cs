using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class BossSpawn : MonoBehaviour
{
    public GameObject bossSpawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            bossSpawn.SetActive(true);

            Destroy(gameObject);


        }

    }
}