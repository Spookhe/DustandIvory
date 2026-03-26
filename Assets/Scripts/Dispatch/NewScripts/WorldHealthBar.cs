using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WorldHealthBar : MonoBehaviour
{
    [Header("Tracking")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 3f, 0);

    [Header("Segments")]
    public Transform segmentContainer;
    public GameObject segmentPrefab;

    [Header("Colors")]
    public Color normalColor = Color.red;
    public Color emptyColor = Color.black;

    private List<Image> segments = new List<Image>();

    // Create segments based on tier
    public void CreateSegments(int count)
    {
        segments.Clear();

        foreach (Transform child in segmentContainer)
            Destroy(child.gameObject);

        for (int i = 0; i < count; i++)
        {
            GameObject seg = Instantiate(segmentPrefab, segmentContainer);
            Image img = seg.GetComponent<Image>();

            img.color = normalColor;
            segments.Add(img);
        }
    }

    // Update segments visually
    public void SetHealth(int remaining)
    {
        for (int i = 0; i < segments.Count; i++)
        {
            if (i < remaining)
                segments[i].color = normalColor;
            else
                segments[i].color = emptyColor;
        }
    }

    void Update()
    {
        if (target != null)
            transform.position = target.position + offset;

        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180f, 0);
    }
}