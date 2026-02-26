using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HubUIManager : MonoBehaviour
{
    public GameObject hubMenu, levelSelect, loadout, shop;
    public TextMeshProUGUI totalMoneyText;

    void Start()
    {
        ShowHub();
    }

    void HideAll()
    {
        hubMenu.SetActive(false);
        levelSelect.SetActive(false);
        loadout.SetActive(false);
        shop.SetActive(false);
    }

    public void ShowHub()
    {
        HideAll();
        hubMenu.SetActive(true);
        UpdateMoneyDisplay();
    }

    public void ShowLevelSelect() { HideAll(); levelSelect.SetActive(true); }
    public void ShowLoadout() { HideAll(); loadout.SetActive(true); }
    public void ShowShop() { HideAll(); shop.SetActive(true); }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void UpdateMoneyDisplay()
    {
        if (GameManager.Instance == null) return;

        totalMoneyText.text = $"<color=green>Money: ${GameManager.Instance.money}</color>";
    }
}
