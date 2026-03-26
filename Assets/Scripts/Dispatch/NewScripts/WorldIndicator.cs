using UnityEngine;

public class WorldIndicator : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2.5f, 0);

    void Update()
    {
        if (target == null) return;

        transform.position = target.position + offset;

        // Always face camera
        transform.forward = Camera.main.transform.forward;
    }
}