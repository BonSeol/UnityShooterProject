using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponSwapUI : MonoBehaviour
{
    public Image weaponSlot1;
    public Image weaponSlot2;
    public Sprite[] weaponIcons; // 무기 아이콘 리스트
    private int currentWeaponIndex = 0;

    void Start()
    {
        UpdateWeaponUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwapWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwapWeapon(1);
        }
    }

    public void SwapWeapon(int weaponIndex)
    {
        if (currentWeaponIndex == weaponIndex)
        {
            return; // 현재 무기와 선택된 무기가 동일하면 스왑을 실행하지 않음
        }

        currentWeaponIndex = weaponIndex % weaponIcons.Length;
        StartCoroutine(AnimateWeaponSwap());
    }

    private IEnumerator AnimateWeaponSwap()
    {
        // 부드럽게 넘기는 애니메이션 (예제)
        float duration = 0.3f;
        Vector3 startPos1 = weaponSlot1.transform.localPosition;
        Vector3 startPos2 = weaponSlot2.transform.localPosition;
        Vector3 endPos1 = startPos1 + new Vector3(0, -100, 0); // 첫 번째 슬롯을 아래로 이동
        Vector3 endPos2 = startPos1;  // 두 번째 슬롯을 첫 번째 슬롯의 시작 위치로 이동

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            weaponSlot1.transform.localPosition = Vector3.Lerp(startPos1, endPos1, progress);
            weaponSlot2.transform.localPosition = Vector3.Lerp(startPos2, endPos2, progress);
            yield return null;
        }

        // 무기 UI 교체
        weaponSlot1.sprite = weaponIcons[currentWeaponIndex];

        // 초기 위치 복귀
        weaponSlot1.transform.localPosition = startPos2; // 첫 번째 슬롯을 두 번째 슬롯의 시작 위치로 이동
        weaponSlot2.transform.localPosition = startPos1; // 두 번째 슬롯을 첫 번째 슬롯의 시작 위치로 이동

        // 슬롯 참조 교체
        Image temp = weaponSlot1;
        weaponSlot1 = weaponSlot2;
        weaponSlot2 = temp;

        // 현재 선택된 무기를 UI 계층 구조의 맨 앞으로 이동
        weaponSlot1.transform.SetAsLastSibling();
    }

    private void UpdateWeaponUI()
    {
        weaponSlot1.sprite = weaponIcons[currentWeaponIndex];
        weaponSlot2.sprite = weaponIcons[(currentWeaponIndex + 1) % weaponIcons.Length];
    }
}