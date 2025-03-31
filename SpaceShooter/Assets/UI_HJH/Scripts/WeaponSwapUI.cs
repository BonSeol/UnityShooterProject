using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponSwapUI : MonoBehaviour
{
    public Image weaponSlot1;
    public Image weaponSlot2;
    public Sprite[] weaponIcons; // ���� ������ ����Ʈ
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
            return; // ���� ����� ���õ� ���Ⱑ �����ϸ� ������ �������� ����
        }

        currentWeaponIndex = weaponIndex % weaponIcons.Length;
        StartCoroutine(AnimateWeaponSwap());
    }

    private IEnumerator AnimateWeaponSwap()
    {
        // �ε巴�� �ѱ�� �ִϸ��̼� (����)
        float duration = 0.3f;
        Vector3 startPos1 = weaponSlot1.transform.localPosition;
        Vector3 startPos2 = weaponSlot2.transform.localPosition;
        Vector3 endPos1 = startPos1 + new Vector3(0, -100, 0); // ù ��° ������ �Ʒ��� �̵�
        Vector3 endPos2 = startPos1;  // �� ��° ������ ù ��° ������ ���� ��ġ�� �̵�

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            weaponSlot1.transform.localPosition = Vector3.Lerp(startPos1, endPos1, progress);
            weaponSlot2.transform.localPosition = Vector3.Lerp(startPos2, endPos2, progress);
            yield return null;
        }

        // ���� UI ��ü
        weaponSlot1.sprite = weaponIcons[currentWeaponIndex];

        // �ʱ� ��ġ ����
        weaponSlot1.transform.localPosition = startPos2; // ù ��° ������ �� ��° ������ ���� ��ġ�� �̵�
        weaponSlot2.transform.localPosition = startPos1; // �� ��° ������ ù ��° ������ ���� ��ġ�� �̵�

        // ���� ���� ��ü
        Image temp = weaponSlot1;
        weaponSlot1 = weaponSlot2;
        weaponSlot2 = temp;

        // ���� ���õ� ���⸦ UI ���� ������ �� ������ �̵�
        weaponSlot1.transform.SetAsLastSibling();
    }

    private void UpdateWeaponUI()
    {
        weaponSlot1.sprite = weaponIcons[currentWeaponIndex];
        weaponSlot2.sprite = weaponIcons[(currentWeaponIndex + 1) % weaponIcons.Length];
    }
}