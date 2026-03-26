using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 12f;
    public float mouseSensitivity = 2f;
    public Transform playerCamera;

    [Header("Arrest UI")]
    public GameObject holdEText;          // Parent ArrestText
    public RectTransform arrestProgressBar; // Green Fill rectangle

    [Header("Arrest Logic")]
    public float arrestRange = 3f;
    public float arrestHoldTime = 2f;

    private float arrestTimer;
    private Poacher currentTarget;
    private CharacterController controller;
    private float verticalRotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Hide Arrest UI at start
        if (holdEText != null)
            holdEText.SetActive(false);

        // Initialize Fill scale
        if (arrestProgressBar != null)
        {
            Vector3 scale = arrestProgressBar.localScale;
            scale.x = 0f; // start empty
            arrestProgressBar.localScale = scale;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleArrest();
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        Vector3 move = transform.right * h + transform.forward * v;
        controller.Move(move * speed * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);

        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleArrest()
    {
        FindKnockedOutPoacher();

        bool canArrest = currentTarget != null;

        // Show/hide Arrest UI
        if (holdEText != null)
            holdEText.SetActive(canArrest);

        if (canArrest)
        {
            if (Input.GetKey(KeyCode.E))
                arrestTimer += Time.deltaTime;

            // Scale Fill to simulate sliding green bar
            if (arrestProgressBar != null)
            {
                float t = Mathf.Clamp01(arrestTimer / arrestHoldTime);
                Vector3 scale = arrestProgressBar.localScale;
                scale.x = t; // grows left to right
                arrestProgressBar.localScale = scale;
            }

            // Arrest when full
            if (arrestTimer >= arrestHoldTime)
            {
                currentTarget.Arrest();
                ResetArrest();
            }

            // Reset if E released early
            if (!Input.GetKey(KeyCode.E))
                ResetArrest();
        }
        else
        {
            ResetArrest();
        }
    }

    void ResetArrest()
    {
        arrestTimer = 0f;
        if (arrestProgressBar != null)
        {
            Vector3 scale = arrestProgressBar.localScale;
            scale.x = 0f; // reset to empty
            arrestProgressBar.localScale = scale;
        }
    }

    void FindKnockedOutPoacher()
    {
        currentTarget = null;
        Collider[] hits = Physics.OverlapSphere(transform.position, arrestRange);
        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("KnockedOut")) continue;
            Poacher p = hit.GetComponent<Poacher>();
            if (p != null)
            {
                currentTarget = p;
                return;
            }
        }
    }

    public bool IsNearKnockedOutPoacher()
    {
        return currentTarget != null;
    }
}