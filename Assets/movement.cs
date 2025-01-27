using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public CharacterController cc;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    [SerializeField] private float Sensitivity;

    [SerializeField] private float speed, walk, run, crouch;
    [SerializeField] private float jumpHeight = 3.0f;

    private Vector3 crouchScale, normalScale;
    private Vector3 velocity;
    private float gravity = -9.81f;

    public bool isMoving, isCrouching, isRunning;

    private float X, Y;

    private void Start()
    {
        speed = walk;
        crouchScale = new Vector3(1, .75f, 1);
        normalScale = new Vector3(1, 1, 1);
        cc = GetComponent<CharacterController>();
        cc.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        #region Camera Limitation Calculator
        // Camera limitation variables
        const float MIN_Y = -60.0f;
        const float MAX_Y = 70.0f;

        X += Input.GetAxis("Mouse X") * (Sensitivity * Time.deltaTime);
        Y -= Input.GetAxis("Mouse Y") * (Sensitivity * Time.deltaTime);

        if (Y < MIN_Y)
            Y = MIN_Y;
        else if (Y > MAX_Y)
            Y = MAX_Y;
        #endregion

        transform.localRotation = Quaternion.Euler(Y, X, 0.0f);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;

        Vector3 move = (forward + right).normalized;

        // Apply movement
        cc.SimpleMove(move * speed);

        // Determines if the speed = run or walk
        if (Input.GetKey(KeyCode.LeftShift ))
            
        {
            _animator.SetBool("idle", false);
            _animator.SetBool("sprint",true);
     
            speed = run;
            isRunning = true;
        }
        // Crouch
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            isRunning = false;
            speed = crouch;
            player.transform.localScale = crouchScale;
        }
        else
        {
            _animator.SetBool("idle", true);
            _animator.SetBool("sprint", false);
            isRunning = false;
            isCrouching = false;
            speed = walk;
            player.transform.localScale = normalScale;
        }

        // Jump
        if (cc.isGrounded)
        {
            velocity.y = 0f; // Reset vertical velocity when grounded

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move the character controller using velocity
        cc.Move(velocity * Time.deltaTime);

        // Detects if the player is moving
        isMoving = cc.velocity.sqrMagnitude > 0.0f ? true : false;
    }
}
