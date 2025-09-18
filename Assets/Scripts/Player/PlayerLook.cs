using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    [Header("Reqs")]
    public Camera cam;

    [Header("Stats")]
    private float xRotation = 0f;
    public float sensitivity = 10f;

    private Vector2 velocity;
    [SerializeField]
    private float accelerationInput = 0.5f;

    private void Start()
    {
        InputManager.Instance.OnLook += SetInput;
        //EventManager.Instance.OnTakeDamage += CameraShake;
    }

    private void LateUpdate()
    {
        ProcessLook();
    }

    private void SetInput(Vector2 _input)
    {
        velocity = Vector2.Lerp(velocity, _input, accelerationInput);
    }

    private void ProcessLook()
    {
        float mouseX = velocity.x;
        float mouseY = velocity.y;

        xRotation -= (mouseY * Time.deltaTime) * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.parent.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * sensitivity);
    }


}
