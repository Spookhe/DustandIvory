using UnityEngine;

public class LoadoutManager : MonoBehaviour
{
    [Header("References")]
    public PlayerWeaponManager weaponManager;

    void Start()
    {
        UpdateWeapon();
    }

    private void SelectWeaponInternal(string weaponName)
    {
        if (!PlayerInventory.Instance.OwnsWeapon(weaponName))
        {
            Debug.LogWarning($"Player does not own {weaponName}!");
            return;
        }

        PlayerInventory.Instance.SelectWeapon(weaponName);
        UpdateWeapon();
    }

    public void SelectTranquilizerPistol() => SelectWeaponInternal("TranquilizerPistol");
    public void SelectTranquilizerRifle() => SelectWeaponInternal("TranquilizerRifle");
    public void SelectTranquilizerSMG() => SelectWeaponInternal("TranquilizerSMG");

    public void UpdateWeapon()
    {
        if (weaponManager != null)
        {
            string selected = PlayerInventory.Instance.GetSelectedWeapon();
            weaponManager.EquipWeapon(selected);
        }
        else
        {
            Debug.LogWarning("WeaponManager reference is missing in LoadoutManager!");
        }
    }
}