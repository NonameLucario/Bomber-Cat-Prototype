using Unity.Mathematics;
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

    [SerializeField] private LayerMask SlideLayer;
    [SerializeField] private LayerMask GroundedLayer;
    [SerializeField] private bool isCanWallJump;

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
        isGrounded = ProcessGrounded();
        jumpBufferTime -= Time.deltaTime;


        ProcessGravity();
        ProcessMoveCC();
        //ProcessMoveRB();
        ProcessCrouchTransition();

        
    }

    private bool ProcessGrounded()
    {
        bool flag = false;

        flag = characterController.isGrounded;
        
        //if(Physics.CapsuleCast(
        //    transform.position, transform.position + (Vector3.up * 0.1f),
        //    characterController.radius, Vector3.down, 0.1f, SlideLayer
        //)) flag = false;

        //UIManager.Instance.devSlideGrounded = Physics.CapsuleCast(
        //    transform.position, transform.position + (Vector3.up * 1f),
        //    characterController.radius, Vector3.down, 0.1f, SlideLayer
        //);


        bool flag2 = Physics.CheckSphere(transform.position, characterController.radius, SlideLayer);
        if (flag2)
        {
            flag = false;
        }
        UIManager.Instance.devSlideGrounded = flag2;
        UIManager.Instance.devGrounded = flag;
        if (flag) { isCanWallJump = true; }
        return flag;
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

        // пока не уверен что эту миханику нужно добавл€ть из-за рокетджампа,
        // вообще забываю что прыжок на по стенке есть... мб надо сделать что то с осущением от этого прыжка
        //if (!isGrounded && CanWallJump()) verticalVelocity = jumpForce;
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

    private bool CanWallJump()
    {
        if (!isCanWallJump) return false;
        Ray ray = new Ray(transform.position, transform.forward);
        bool flag = Physics.SphereCast(ray, characterController.radius, 0.7f, GroundedLayer);
        isCanWallJump = false;
        return flag;
    }

    //сделать проверку чтобы метод срабатывал когда игрок находитс€ над бомбой 
    public void TryRocketJump(Vector3 forcePosition)
    {
        float distance = Vector3.Distance(transform.position, forcePosition);
        verticalVelocity = jumpForce  * 2 * (1 -(distance / 10f));
        //Debug.Log($"fp:{forcePosition}; vv:{1 - (distance / 10f)};");
    }
}
