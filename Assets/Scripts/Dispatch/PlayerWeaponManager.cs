using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public GameObject TranquilizerPistol;
    public GameObject TranquilizerRifle;
    public GameObject TranquilizerSMG;

    void Start()
    {
        // Equip the weapon the player currently has selected
        string selectedWeapon = PlayerInventory.Instance.GetSelectedWeapon();

        // If no weapon selected yet, default to Pistol
        if (string.IsNullOrEmpty(selectedWeapon))
        {
            selectedWeapon = "TranquilizerPistol";
            PlayerInventory.Instance.AddWeapon(selectedWeapon); // ensure player owns the default weapon
            PlayerInventory.Instance.SelectWeapon(selectedWeapon);
        }

        EquipWeapon(selectedWeapon);
    }

    public void EquipWeapon(string weaponName)
    {
        TranquilizerPistol.SetActive(false);
        TranquilizerRifle.SetActive(false);
        TranquilizerSMG.SetActive(false);

        switch (weaponName)
        {
            case "TranquilizerPistol":
                TranquilizerPistol.SetActive(true);
                break;
            case "TranquilizerRifle":
                TranquilizerRifle.SetActive(true);
                break;
            case "TranquilizerSMG":
                TranquilizerSMG.SetActive(true);
                break;
            default:
                Debug.LogWarning($"Unknown weapon: {weaponName}");
                TranquilizerPistol.SetActive(true);
                break;
        }
    }
}