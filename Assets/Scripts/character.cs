using UnityEngine;

public class character : MonoBehaviour
{
    
    public Camera playerCamera; // Defines the camera
    public float speed = 5f; // Defines speed of the character
    public float jumpPower = 7f; // Defines jump power of the character
    public float gravity = 9.8f; // Defines gravity of the character
    public float lookSpeed = 2f; // Defines look speed of the character
    public float lookXLimit = 45.0f; // Defines look X limit of the character
    public float defaultHeight = 2f; // Defines default height of the character

    private CharacterController characterController; // Defines character controller

    private Vector3 moveDirection = Vector3.zero; // Defines move direction of the character
    private float rotationX = 0; // Defines rotation X of the character


    private bool canMove = true; // Defines if the character can move
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>(); // Gets the character controller component
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor
        Cursor.visible = false; // Makes the cursor invisible
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward); // Defines forward vector
        Vector3 right = transform.TransformDirection(Vector3.right); // Defines right vector

        float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0; // Defines current speed X
        float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0; // Defines current speed Y
        float movementDirectionY = moveDirection.y; // Defines movement direction Y
        moveDirection = (forward * curSpeedX) + (right * curSpeedY); // Defines move direction

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded) // If jump button is pressed and character can move and character is grounded
        {
            moveDirection.y = jumpPower; // Sets move direction Y to jump power
        }
        else
        {
            moveDirection.y = movementDirectionY; // Sets move direction Y to movement direction Y
        }

        if (!characterController.isGrounded) // If character is not grounded
        {
            moveDirection.y -= gravity * Time.deltaTime; // Sets move direction Y to gravity
        }
        
        characterController.Move(moveDirection * Time.deltaTime); // Moves the character controller

        if (canMove) // If character can move
        {
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeed; // Sets rotation X to mouse Y
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit); // Clamps rotation X
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0); // Sets player camera local rotation
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0); // Sets transform rotation
        }
    }
}
