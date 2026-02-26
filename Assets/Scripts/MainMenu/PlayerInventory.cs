using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    private HashSet<string> ownedWeapons = new HashSet<string>();
    private string selectedWeapon;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Default weapon
        AddWeapon("TranquilizerPistol");
        SelectWeapon("TranquilizerPistol");
        DontDestroyOnLoad(gameObject);
    }

    public void AddWeapon(string weaponName)
    {
        ownedWeapons.Add(weaponName);
    }

    public bool OwnsWeapon(string weaponName)
    {
        return ownedWeapons.Contains(weaponName);
    }

    public void SelectWeapon(string weaponName)
    {
        if (OwnsWeapon(weaponName))
            selectedWeapon = weaponName;
        else
            Debug.LogWarning("Cannot select weapon not owned: " + weaponName);
    }

    public string GetSelectedWeapon()
    {
        return selectedWeapon;
    }
}