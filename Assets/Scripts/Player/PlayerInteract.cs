using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;

    private float interactDistance = 3f;
    [SerializeField] private LayerMask interactMask;




    private void Start()
    {
        cam = Camera.main;
        InputManager.Instance.OnInteract += Interact;
    }
    private void Update()
    {
        UpdateInteractMessage();
    }

    private void UpdateInteractMessage()
    {
        //UIManager.Instance.OnUpdatePrompt?.Invoke(string.Empty, false);
        //if (ProcessRaycast(out Iteractable _iteractable))
        //{
        //    UIManager.Instance.OnUpdatePrompt?.Invoke(_iteractable.prompMassage, true);
        //}
    }

    private void Interact()
    {
        
        if (ProcessRaycast(out Interactable _interactable))
        {
            _interactable.Interact();
        }
    }

    private bool ProcessRaycast(out Interactable _interactable)
    {
        _interactable = null;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, interactDistance, interactMask))
        {
            return hitInfo.collider.TryGetComponent(out _interactable);
        }
        return false;
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Debug.Log(hit + " - normal: " + hit.normal);
    //}
}
