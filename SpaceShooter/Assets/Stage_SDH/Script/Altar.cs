using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] private GameObject flameObject; // �Ҳ� ������Ʈ

    // �Ҳ� Ȱ��ȭ �Լ�
    public void ActivateFlame()
    {
        if (flameObject != null)
        {
            flameObject.SetActive(true); // �Ҳ� ������Ʈ Ȱ��ȭ
        }
    }
}
