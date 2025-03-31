using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject dropItem; // 드롭할 아이템 프리팹

    public void Drop()
    {
        Debug.Log("아이템 드롭 호출됨");
        if (dropItem != null)
        {
            Instantiate(dropItem, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }
}
