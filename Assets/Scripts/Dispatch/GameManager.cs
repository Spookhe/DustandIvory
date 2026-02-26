using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Persistent Player Data")]
    public int money = 0;

    [Header("Mission Rewards")]
    public int rewardPerPoacher = 100;

    [Header("Loadout Data")]
    public bool ownsTranquilizerRifle = false;
    public bool ownsTranquilizerSMG = false;
    public string selectedWeapon = "Pistol"; // default

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddMoney(int amount)
    {
        money += Mathf.Max(0, amount);
    }

    public void SpendMoney(int amount)
    {
        money = Mathf.Max(0, money - amount);
    }

    public void ReturnToHub()
    {
        SceneManager.LoadScene("HubScene");
    }
}