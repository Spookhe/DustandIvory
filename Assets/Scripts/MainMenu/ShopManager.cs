using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private void BuyWeapon(string weaponName, int cost, Button buttonToHide = null)
    {
        if (GameManager.Instance.money >= cost && !PlayerInventory.Instance.OwnsWeapon(weaponName))
        {
            GameManager.Instance.AddMoney(-cost);
            PlayerInventory.Instance.AddWeapon(weaponName);
            Debug.Log($"{weaponName} purchased!");

            if (buttonToHide != null)
                buttonToHide.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"Cannot buy {weaponName}: Not enough money or already owned.");
        }
    }

    public void BuyTranquilizerRifle(Button buttonToHide = null)
        => BuyWeapon("TranquilizerRifle", 1000, buttonToHide);

    public void BuyTranquilizerSMG(Button buttonToHide = null)
        => BuyWeapon("TranquilizerSMG", 2000, buttonToHide);

    public void SelectWeapon(string weaponName)
    {
        if (PlayerInventory.Instance.OwnsWeapon(weaponName))
        {
            PlayerInventory.Instance.SelectWeapon(weaponName);
            Debug.Log($"{weaponName} selected for next mission!");
        }
        else
        {
            Debug.LogWarning($"Player does not own {weaponName}!");
        }
    }
}