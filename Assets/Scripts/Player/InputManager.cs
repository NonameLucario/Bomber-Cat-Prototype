using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public Action<Vector2> OnMove;
    public Action<Vector2> OnLook;
    public Action OnJump;
    public Action OnCrouch;
    public Action<bool> OnSprint;

    public Action<int> OnSlot1;
    public Action<int> OnSlot2;
    public Action OnAttackStarted;
    public Action OnAltAttackStarted;
    public Action OnAltAttackCanceled;
    public Action OnDrop;
    public Action OnInteract;

    [Header("Move")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference crouchAction;
    [SerializeField] private InputActionReference sprintAction;

    [Header("Invetory")]
    //[SerializeField] private InputActionReference slot1Action;
    //[SerializeField] private InputActionReference slot2Action;
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference altAttackAction;
    //[SerializeField] private InputActionReference dropAction;
    [SerializeField] private InputActionReference interactAction;

    private void Awake()
    {
        Instance = this;

        Cursor.lockState = CursorLockMode.Locked;

        jumpAction.action.started += jumpStarted => OnJump?.Invoke();
        sprintAction.action.started += sprintStarted => OnSprint?.Invoke(true);
        sprintAction.action.canceled += sprintCanceled => OnSprint?.Invoke(false);
        crouchAction.action.started += crouchStarted => OnCrouch?.Invoke();

        attackAction.action.started += useStarted => OnAttackStarted?.Invoke();
        //useAction.action.canceled += useCanceled => OnUseCanceled?.Invoke();
        //slot1Action.action.started += slot1Started => OnSlot1?.Invoke(1);
        //slot2Action.action.started += slot2Started => OnSlot2?.Invoke(2);
        //dropAction.action.started += interactStarted => OnDrop?.Invoke();
        interactAction.action.started += interactStarted => OnInteract?.Invoke();
    }

    private void Update()
    {
        OnMove?.Invoke(moveAction.action.ReadValue<Vector2>());
        OnLook?.Invoke(lookAction.action.ReadValue<Vector2>());
    }
}
