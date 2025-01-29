using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public CharacterController cc;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    [SerializeField] private float Sensitivity;

    [SerializeField] private float walkSpeed = 5f, runSpeed = 10f, crouchSpeed = 2.5f;
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private float jumpBufferTime = 0.2f;

    private Vector3 crouchScale = new Vector3(1, 0.75f, 1);
    private Vector3 normalScale = Vector3.one;
    private Vector3 velocity;
    private float gravity = -9.81f;
    private float lastJumpPressedTime = -1f;

    private float currentSpeed;
    private bool isGrounded;
    private float X, Y;

    private void Start()
    {
        currentSpeed = walkSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleCamera();
        HandleMovement();
        HandleJump();
        UpdateAnimations();
        ApplyGravity();
    }

    private void HandleCamera()
    {
        const float MIN_Y = -60f, MAX_Y = 70f;

        X += Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        Y -= Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
        Y = Mathf.Clamp(Y, MIN_Y, MAX_Y);

        transform.localRotation = Quaternion.Euler(Y, X, 0);
    }

    private void HandleMovement()
    {
        isGrounded = cc.isGrounded;
        Vector3 move = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;

        Vector3 horizontalVelocity = move * currentSpeed;
        velocity.x = horizontalVelocity.x;
        velocity.z = horizontalVelocity.z;

        HandleSpeedChanges();
    }

    private void HandleSpeedChanges()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = crouchSpeed;
            player.transform.localScale = crouchScale;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
            player.transform.localScale = normalScale;
        }
        else
        {
            currentSpeed = walkSpeed;
            player.transform.localScale = normalScale;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            lastJumpPressedTime = Time.time;

        if (isGrounded)
        {
            velocity.y = -2f;

            if (Time.time - lastJumpPressedTime <= jumpBufferTime)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                _animator.SetTrigger("Jump");
                lastJumpPressedTime = -1f;
            }
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
            velocity.y += gravity * Time.deltaTime;

        cc.Move(velocity * Time.deltaTime);
    }

    private void UpdateAnimations()
    {
        _animator.SetBool("Grounded", isGrounded);

        if (isGrounded)
        {
            bool isMoving = cc.velocity.magnitude > 0.1f;

            _animator.SetBool("Sprint", isMoving && currentSpeed == runSpeed);
            _animator.SetBool("Walk", isMoving && currentSpeed == walkSpeed);
            _animator.SetBool("Crouch", currentSpeed == crouchSpeed);
            _animator.SetBool("Idle", !isMoving);
        }
        else
        {
            _animator.SetBool("Sprint", false);
            _animator.SetBool("Walk", false);
            _animator.SetBool("Crouch", false);
            _animator.SetBool("Idle", false);
        }
    }
}