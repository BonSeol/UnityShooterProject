using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject dropItem; // ����� ������ ������

    public void Drop()
    {
        Debug.Log("������ ��� ȣ���");
        if (dropItem != null)
        {
            Instantiate(dropItem, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }
}
