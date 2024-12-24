using System.Collections;
using TMPro;
using UnityEngine;

public class elevator : MonoBehaviour
{
    private GameObject elevatorBase;                //Defines elevator base
    private GameObject elevatorDoorLeft;            //Defines left elevator door
    private GameObject elevatorDoorRight;           //Defines right elevator door
    private GameObject doorLeftClosed;              //Defines left door closed node
    private GameObject doorRightClosed;             //Defines right door closed node
    private GameObject doorLeftOpen;                //Defines left door open node
    private GameObject doorRightOpen;               //Defines right door open node
    private GameObject floorCounter;                //Defines floor counter

    private Vector3 lastPosition;
    public Vector3 Movement { get; private set; }   // Movement of elevator

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
        floorCounter = GameObject.Find("FloorCounter");

        lastPosition = transform.position;
    }

    void Update()
    {
        // Update floorCounter text with a floorNodes value whenever doorRightClosed overlaps a floorNode
        // lastNode is the last Floor Node doorRightClosed collided with
        GameObject lastNode = null;

        /*
        if any Floor Node tagged object collides with doorRightClosed, get the floorNumber of the Floor Node
        and update the floorCounter text with the floorNumber
        */
        foreach (GameObject floorNode in GameObject.FindGameObjectsWithTag("FloorNode"))
        {
            if (doorRightClosed.GetComponent<Collider>().bounds.Intersects(floorNode.GetComponent<Collider>().bounds))
            {
                lastNode = floorNode;
                floorCounter.GetComponent<TextMeshPro>().text = lastNode.GetComponent<floorNode>().floorNumber.ToString();
            }
        }
        
        

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
        /*
        When function OnButtonPressed() is called, move gameObjects elevatorDoorLeft 
        and elevatorDoorRight to the positions of doorLeftOpen and doorRightOpen
        */
        Debug.Log("OpenDoors() called");
        StartCoroutine(MoveToPosition(elevatorDoorLeft, doorLeftOpen.transform.position, doorSpeed));
        yield return StartCoroutine(MoveToPosition(elevatorDoorRight, doorRightOpen.transform.position, doorSpeed));
    }

    // Close doors when elevator call button is pressed
    public IEnumerator CloseDoors()
    {
        /*
        When function OnButtonPressed() is called, move gameObjects elevatorDoorLeft and 
        elevatorDoorRight to the positions of doorLeftClosed and doorRightClosed
        */
        Debug.Log("CloseDoors() called");
        StartCoroutine(MoveToPosition(elevatorDoorLeft, doorLeftClosed.transform.position, doorSpeed));
        yield return StartCoroutine(MoveToPosition(elevatorDoorRight, doorRightClosed.transform.position, doorSpeed));
    }

    

    public IEnumerator GoToFloor(int floorNumber)
    {
        // Close elevator doors
        Debug.Log($"GoToFloor({floorNumber}) called");
        yield return StartCoroutine(CloseDoors());
        
        // On floor called, set parent of Level and other floor nodes to floor node of floor called
        GameObject[] floorNodes = GameObject.FindGameObjectsWithTag("FloorNode");   // Find all floor nodes
        GameObject floorCalled = GameObject.Find($"floorNode{floorNumber}");        // Find floor node that matches floor number
        GameObject levelContainer = GameObject.Find("Level");                       // Find level container
        levelContainer.transform.SetParent(floorCalled.transform);                  // Set level container parent to floor called
        
        // Set parent of other floor nodes to floor called
        foreach (GameObject floorNode in floorNodes)
        {
            if (floorNode != floorCalled)
            {
                floorNode.transform.SetParent(floorCalled.transform);
            }
        }

        // Move level to elevator base
        yield return StartCoroutine(MoveToPosition(floorCalled, doorRightClosed.transform.position, elevatorSpeed));

        // Return all objects to their original parents
        levelContainer.transform.SetParent(null);
        foreach (GameObject floorNode in floorNodes)
        {
            floorNode.transform.SetParent(null);
        }

        // Open elevator doors
        yield return StartCoroutine(OpenDoors());
    }
}
