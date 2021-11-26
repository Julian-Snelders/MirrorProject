using Mirror;
using UnityEngine;

public class PlayerMovementController : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 7.5f;
    [SerializeField] private CharacterController controller = null;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    float rotationX = 0;

    private Vector2 previousInput;

    private Controls controls;

    private Controls Controls
    {
        get
        {
            if(controls != null) { return controls; }
            return controls = new Controls();
        }
    }

    public override void OnStartAuthority()
    {
        Cursor.lockState = CursorLockMode.Locked;

        enabled = true;

        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        Controls.Player.Move.canceled += ctx => ResetMovement();

     
        playerCamera.gameObject.SetActive(true);
        
    }

    [ClientCallback]
    private void OnEnable() => Controls.Enable();
    [ClientCallback]
    private void OnDisable() => Controls.Disable();
    [ClientCallback]
    private void Update() => Move();

    [Client]
    private void SetMovement(Vector2 movement) => previousInput = movement;

    [Client]
    private void ResetMovement() => previousInput = Vector2.zero;

    [Client]

    private void Move()
    {
        Vector3 right = controller.transform.right;
        Vector3 forward = controller.transform.forward;
        right.y = 0f;
        forward.y = 0f;

        Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y;

        controller.Move(movement * movementSpeed * Time.deltaTime);



        //camera
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

}
