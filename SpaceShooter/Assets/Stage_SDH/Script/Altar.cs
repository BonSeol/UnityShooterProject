using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] private GameObject flameObject; // 불꽃 오브젝트

    // 불꽃 활성화 함수
    public void ActivateFlame()
    {
        if (flameObject != null)
        {
            flameObject.SetActive(true); // 불꽃 오브젝트 활성화
        }
    }
}
