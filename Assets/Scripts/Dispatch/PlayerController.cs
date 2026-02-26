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

    [Header("Arrest")]
    public float arrestRange = 3f;
    public float arrestHoldTime = 2f;
    public Image arrestProgressBar;

    float verticalRotation;
    float arrestTimer;
    Poacher currentTarget;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

        if (currentTarget != null && Input.GetKey(KeyCode.E))
        {
            arrestTimer += Time.deltaTime;

            if (arrestProgressBar != null)
                arrestProgressBar.fillAmount = arrestTimer / arrestHoldTime;

            if (arrestTimer >= arrestHoldTime)
            {
                // Arrest poacher
                currentTarget.Arrest();
                ResetArrest();
            }
        }
        else
        {
            ResetArrest();
        }
    }

    void ResetArrest()
    {
        arrestTimer = 0f;
        if (arrestProgressBar != null) arrestProgressBar.fillAmount = 0f;
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