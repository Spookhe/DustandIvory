using UnityEngine;

public class PoacherTier : MonoBehaviour
{
    public enum Tier { Tier1, Tier2, Tier3 }
    public Tier tier = Tier.Tier1;

    private Poacher poacher;

    void Awake()
    {
        poacher = GetComponent<Poacher>();

        switch (tier)
        {
            case Tier.Tier1:
                poacher.maxHits = 1;
                break;
            case Tier.Tier2:
                poacher.maxHits = 2;
                break;
            case Tier.Tier3:
                poacher.maxHits = 3;
                break;
        }
    }
}