using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float crouchSpeed = 2f;

    [Header("Jump and Fall")]
    //хз добал€ть койтотайм... пока не почувтвовал необходимости
    [SerializeField] private const float jumpBufferMaxTime = 0.2f; // 0.2f как будто идельный буфер дл€ прыжка
    private float jumpBufferTime = 0f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = -12f;
    [SerializeField] private float initialFallVelocity = -2f;

    [Header("Crouching")]
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float crouchingHeight = 0.9f;
    [SerializeField] private float crouchTransitionSpeed = 10f;
    [SerializeField] private float cameraOffset = 0.4f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CharacterController characterController;
    //[SerializeField] private Rigidbody rb;


    private float accelerationInput = 0.1f;

    private Vector2 velovityInput;
    private bool isGrounded;
    private bool isRunning;
    private bool isCrouching;
    private float verticalVelocity;
    private float targetHeight;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        InputManager.Instance.OnMove += SetInput;
        InputManager.Instance.OnSprint += Sprint;
        InputManager.Instance.OnJump += Jump;
        InputManager.Instance.OnCrouch += Crouch;

        targetHeight = standingHeight;
    }

    private void Update()
    {
        isGrounded = characterController.isGrounded;
        jumpBufferTime -= Time.deltaTime;


        ProcessGravity();
        ProcessMoveCC();
        //ProcessMoveRB();
        ProcessCrouchTransition();

        
    }

    private void FixedUpdate()
    {
        TryJumpWithBufferTime();
    }

    // InputManager =>
    private void Crouch()
    {
        if (isCrouching)
        {
            if (!CanStandUp()) return;

            targetHeight = standingHeight;
        }
        else
        {
            targetHeight = crouchingHeight;
        }
        isCrouching = !isCrouching;
    }

    private bool CanStandUp()
    {
        return !Physics.CapsuleCast(
            transform.position,
            transform.position + (Vector3.up * 0.1f),
            characterController.radius, Vector3.up, standingHeight
            );
    }
    // InputManager =>
    private void Jump()
    {
        jumpBufferTime = jumpBufferMaxTime;
        
    }

    private void TryJumpWithBufferTime()
    {
        if (jumpBufferTime <= 0) return;
        if (isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }
    

    // InputManager =>
    private void Sprint(bool _input)
    {
        isRunning = _input;
    }

    private void ProcessGravity()
    {
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = initialFallVelocity;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }
    //better do not use rb
    private void ProcessMoveRB()
    {
        
    }

    private void ProcessMoveCC()
    {
        var move = transform.TransformDirection(new Vector3(velovityInput.x, 0, velovityInput.y));
        var currentSpeed = isCrouching ? crouchSpeed : isRunning ? runSpeed : walkSpeed;
        var finalMove = move * currentSpeed;
        finalMove.y = verticalVelocity;

        var collisions = characterController.Move(finalMove * Time.deltaTime);
        if ((collisions & CollisionFlags.Above) != 0)
        {
            verticalVelocity = initialFallVelocity;
        }
    }
    // InputManager =>
    private void SetInput(Vector2 _input)
    {
        velovityInput = Vector2.Lerp(velovityInput, _input, accelerationInput);
    }

    private void ProcessCrouchTransition()
    {
        var currentHeight = characterController.height;
        if (Mathf.Abs(currentHeight - targetHeight) < 0.01f)
        {
            characterController.height = currentHeight;
            return; // Good height!
        }

        var newHeight = Mathf.Lerp(currentHeight, targetHeight, crouchTransitionSpeed * Time.deltaTime);
        characterController.height = newHeight;
        characterController.center = Vector3.up * (newHeight * 0.5f);

        var cameraTargetPosition = cameraTransform.localPosition;
        cameraTargetPosition.y = targetHeight - cameraOffset;
        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition, cameraTargetPosition, crouchTransitionSpeed * Time.deltaTime);
    }

    // лучше сделать толкание придметов так, чем не делать это использу€ rb... ну нахуй ебатьс€ с rb
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.rigidbody) return;
        if (hit.moveDirection.y < -0.3F) return;
        float pushPower = 2.0F;
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        hit.rigidbody.linearVelocity = pushDir * pushPower;
    }
}
