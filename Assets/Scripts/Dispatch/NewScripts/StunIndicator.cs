using UnityEngine;

public class StunIndicator : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 4f, 0f);
    public GameObject indicator;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = target.position + offset;

        transform.LookAt(Camera.main.transform);
        transform.Rotate(0f, 180f, 0f);
    }

    public void SetStunned(bool isStunned)
    {
        if (indicator != null)
            indicator.SetActive(isStunned);
    }
}