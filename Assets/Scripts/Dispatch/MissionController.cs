using UnityEngine;
using TMPro;

public class MissionController : MonoBehaviour
{
    public static MissionController Instance;

    public GameObject endPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI spaceText;

    public int poachersRemaining { get; private set; }
    public int poachersArrested { get; private set; }
    public int animalsPoached { get; private set; }
    public int animalsRemaining { get; private set; }

    private bool missionEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        missionEnded = false;

        if (endPanel != null)
            endPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!missionEnded) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.ReturnToHub();
        }
    }


    public void RegisterPoacher()
    {
        poachersRemaining++;
    }

    public void RegisterAnimal()
    {
        animalsRemaining++;
    }


    public void PoacherArrested()
    {
        if (missionEnded) return;

        poachersArrested++;
        poachersRemaining = Mathf.Max(0, poachersRemaining - 1);

        if (poachersRemaining <= 0)
            EndMission(true);
    }

    public void AnimalPoached()
    {
        if (missionEnded) return;

        animalsPoached++;
        animalsRemaining = Mathf.Max(0, animalsRemaining - 1);

        if (animalsRemaining <= 0)
            EndMission(false);
    }


    private void EndMission(bool success)
    {
        missionEnded = true;

        Poacher[] poachers = FindObjectsOfType<Poacher>();

        foreach (Poacher p in poachers)
        {
            p.enabled = false;

            var agent = p.GetComponent<UnityEngine.AI.NavMeshAgent>();

            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.ResetPath();
                agent.enabled = false;
            }

            if (p.anim != null)
            {
                p.anim.SetBool("isWalking", false);
                p.anim.SetBool("isAttacking", false);
            }
        }

        Time.timeScale = 0f;

        if (endPanel != null)
            endPanel.SetActive(true);

        resultText.text = success ? "All Poachers Arrested!" : "All Animals Poached!";

        int earned = success ? poachersArrested * GameManager.Instance.rewardPerPoacher : 0;

        moneyText.text = $"Money Earned: ${earned}";
        GameManager.Instance.AddMoney(earned);

        spaceText.text = "Press SPACE to continue";

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}