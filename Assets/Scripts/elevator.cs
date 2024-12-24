using System.Collections;
using UnityEngine;

public class elevator : MonoBehaviour
{
    private GameObject elevatorBase; //Defines elevator base
    private GameObject elevatorDoorLeft; //Defines left elevator door
    private GameObject elevatorDoorRight; //Defines right elevator door
    private GameObject doorLeftClosed; //Defines left door closed node
    private GameObject doorRightClosed; //Defines right door closed node
    private GameObject doorLeftOpen; //Defines left door open node
    private GameObject doorRightOpen; //Defines right door open node
    private Rigidbody rb;

    private Vector3 lastPosition;
    public Vector3 Movement { get; private set; }  // Movement of elevator

    public float doorSpeed = 0.5f;
    public float elevatorSpeed = 2f;

    void Start()
    {
        elevatorBase = GameObject.Find("elevatorFloor");
        elevatorDoorLeft = GameObject.Find("elevatorDoorLeft");
        elevatorDoorRight = GameObject.Find("elevatorDoorRight");
        doorLeftClosed = GameObject.Find("doorLeftClosed");
        doorRightClosed = GameObject.Find("doorRightClosed");
        doorLeftOpen = GameObject.Find("doorLeftOpen");
        doorRightOpen = GameObject.Find("doorRightOpen");
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        Movement = transform.position - lastPosition;
        lastPosition = transform.position;
    }

    
    private IEnumerator MoveToPosition(GameObject movingItem, Vector3 targetPosition, float speed)
    {
        // While the distance between the moving item and the target position is greater than 0.001f
        while (Vector3.Distance(movingItem.transform.position, targetPosition) > 0.001f)
        {
            // Move the moving item smoothly towards the target position
            movingItem.transform.position = Vector3.MoveTowards(movingItem.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        // Snap to the exact target position to avoid overshooting
        movingItem.transform.position = targetPosition;
    }
    
    // Open doors when elevator call button is pressed
    public IEnumerator OpenDoors()
    {
        Debug.Log("OpenDoors() called");

        // When function OnButtonPressed() is called, move gameObjects elevatorDoorLeft and elevatorDoorRight to the positions
        // of doorLeftOpen and doorRightOpen
        StartCoroutine(MoveToPosition(elevatorDoorLeft, doorLeftOpen.transform.position, doorSpeed));
        yield return StartCoroutine(MoveToPosition(elevatorDoorRight, doorRightOpen.transform.position, doorSpeed));
    }

    // Close doors when elevator call button is pressed
    public IEnumerator CloseDoors()
    {
        Debug.Log("CloseDoors() called");

        // When function OnButtonPressed() is called, move gameObjects elevatorDoorLeft and elevatorDoorRight to the positions
        // of doorLeftClosed and doorRightClosed
        StartCoroutine(MoveToPosition(elevatorDoorLeft, doorLeftClosed.transform.position, doorSpeed));
        yield return StartCoroutine(MoveToPosition(elevatorDoorRight, doorRightClosed.transform.position, doorSpeed));
    }

    

    public IEnumerator GoToFloor(int floorNumber)
    {
        yield return StartCoroutine(CloseDoors());

        Debug.Log($"eleLeftDoor right: {elevatorDoorLeft.transform.right}");
        Debug.Log($"eleRightDoor right: {elevatorDoorRight.transform.right}");
        Debug.Log($"doorLeftClosed right: {doorLeftClosed.transform.right}");
        Debug.Log($"doorRightClosed right: {doorRightClosed.transform.right}");
        
        Debug.Log($"GoToFloor({floorNumber}) called");
        
        // Get a list of all the floor nodes
        GameObject[] floorNodes = GameObject.FindGameObjectsWithTag("FloorNode");

        Debug.Log($"floorNodes: {floorNodes}");

        // Loop through all the floor nodes
        for (int i = 0; i < floorNodes.Length; i++)
        {
            Debug.Log($"floorNodes[{i}]: {floorNodes[i]}");
            Debug.Log($"floorNodes[{i}] value: {floorNodes[i].GetComponent<floorNode>().floorNumber}");

            int currentFloor = floorNodes[i].GetComponent<floorNode>().floorNumber;

            if (currentFloor == floorNumber)
            {
                Debug.Log($"currentFloor: {currentFloor}");
                Debug.Log($"floorNumber: {floorNumber}");
                Debug.Log("Elevator is going to the floor node that matches the floor number");

                yield return StartCoroutine(MoveToPosition(elevatorBase, floorNodes[i].transform.position, elevatorSpeed)); 
                yield return StartCoroutine(OpenDoors());
                break;
            }
        }
    }
}
