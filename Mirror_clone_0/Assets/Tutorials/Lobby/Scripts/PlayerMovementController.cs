using Mirror;
using UnityEngine;

public class PlayerMovementController : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 7.5f;            // walking Speed
    [SerializeField] private float speed;                           // default speed variable
    [SerializeField] private float sprintSpeed = 12.5f;             // sprinting speed
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
 
    public override void OnStartAuthority() // called on object (Player) that has authority over this gameObject
    {
        GetComponent<NetworkIdentity>().isLocalPlayer = true;
        Cursor.lockState = CursorLockMode.Locked;      // locks cursor

        enabled = true;

        transform.GetComponent<jump>().enabled = true;  // enable jump script
       // transform.GetComponent<placeItems>().enabled = true; // enable item place script

        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>()); // search controls.player.move and read the vector2 value
        Controls.Player.Move.canceled += ctx => ResetMovement();

     
        playerCamera.gameObject.SetActive(true);        // your playercamera will be actie for you
        
    }

    [ClientCallback]               // only listen to clients. (Server doesnt have input. client does)
    private void OnEnable() => Controls.Enable();                    //set active input system
    [ClientCallback]
    private void OnDisable() => Controls.Disable();                  // deactivate input system
    [ClientCallback]
    private void Update()
    {
        if (hasAuthority)
        {
                Move(); // next method Move
                Look();
         
        }
    }

    [Client]
    private void SetMovement(Vector2 movement) => previousInput = movement;

    [Client]
    private void ResetMovement() => previousInput = Vector2.zero;

    [Client]
    public void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // check for sprint
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = movementSpeed;
        }

        Vector3 right = controller.transform.right;
        Vector3 forward = controller.transform.forward;
        right.y = 0f;
        forward.y = 0f;

        Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y;

        controller.Move(movement * speed * Time.deltaTime);
    }

    [Client]
    public void Look()
    {
            //camera 
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
      
    }
   

}
