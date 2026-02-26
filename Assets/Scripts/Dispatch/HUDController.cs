using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI poachersRemainingText;
    public TextMeshProUGUI poachersArrestedText;
    public TextMeshProUGUI animalsPoachedText;
    public TextMeshProUGUI holdEText;

    public PlayerController player;

    void LateUpdate()
    {
        if (MissionController.Instance == null) return;

        poachersRemainingText.text = $"Poachers Remaining: {MissionController.Instance.poachersRemaining}";
        poachersArrestedText.text = $"Poachers Arrested: {MissionController.Instance.poachersArrested}";
        animalsPoachedText.text = $"Animals Poached: {MissionController.Instance.animalsPoached}";

        // Show "Hold E to arrest" if player is near a knocked-out poacher
        holdEText.gameObject.SetActive(player != null && player.IsNearKnockedOutPoacher());
    }
}