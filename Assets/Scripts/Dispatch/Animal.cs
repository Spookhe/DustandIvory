using UnityEngine;
using System.Collections;

public class Animal : MonoBehaviour
{
    public float poachTime = 5f;

    private bool poached = false;

    void Start()
    {
        MissionController.Instance?.RegisterAnimal();
    }

    public void StartPoaching()
    {
        if (!poached)
            StartCoroutine(Poach());
    }

    private IEnumerator Poach()
    {
        yield return new WaitForSeconds(poachTime);

        if (poached) yield break;

        poached = true;

        MissionController.Instance?.AnimalPoached();
        Destroy(gameObject);
    }
}