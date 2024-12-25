using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class elevator : MonoBehaviour
{
    private GameObject elevatorBase;                    //Defines elevator base
    private GameObject elevatorDoorLeft;                //Defines left elevator door
    private GameObject elevatorDoorRight;               //Defines right elevator door
    private GameObject doorLeftClosed;                  //Defines left door closed node
    private GameObject doorRightClosed;                 //Defines right door closed node
    private GameObject doorLeftOpen;                    //Defines left door open node
    private GameObject doorRightOpen;                   //Defines right door open node
    public GameObject floorCounter;                     //Defines floor counter
    private bool isMoving = false;                      //Defines if elevator is moving
    private bool isDoorOpen = false;                    //Defines if elevator door is open
    private Queue<int> floorQueue = new Queue<int>();   //Defines floor queue

    private Vector3 lastPosition;
    public Vector3 Movement { get; private set; }       // Movement of elevator

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
        isDoorOpen = true;
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
        isDoorOpen = false;
    }

    

    public IEnumerator GoToFloor(int floorNumber, string source)
    {
        floorQueue.Enqueue(floorNumber);
        
        // If elevator is moving, return
        if (isMoving)
        {
            yield break;
        }
        
        // If the Queue is not empty...
        while (floorQueue.Count > 0)
        {
            
            isMoving = true;
            int nextFloor = floorQueue.Dequeue();

            // Move to next floor in queue
            // if elevatorButton is pressed
            if (source == "elevatorButtons") 
            {
                // Close elevator doors
                Debug.Log($"GoToFloor({nextFloor}) called");
                yield return StartCoroutine(CloseDoors());
                isDoorOpen = false;
                
                Debug.Log($"(GTF) Button {nextFloor} was pressed");
                
                // On elevator button press, set parent of Level and other floor nodes to floor node of floor called
                GameObject[] floorNodes = GameObject.FindGameObjectsWithTag("FloorNode");   // Find all floor nodes
                GameObject floorCalled = GameObject.Find($"floorNode{nextFloor}");        // Find floor node that matches floor number
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
                isDoorOpen = true;
            }

            // On call button press, move elvator to floor node that matches the floor number
            else if (source == "elevatorCallButton")
            {
                if (isDoorOpen)
                {
                    yield return StartCoroutine(CloseDoors());
                }
                Debug.Log($"(GTF) Call Button {nextFloor} was pressed");

                GameObject floorCalled = GameObject.Find($"floorNode{nextFloor}");        // Find floor node that matches floor number

                yield return StartCoroutine(MoveToPosition(elevatorBase, floorCalled.transform.position, elevatorSpeed));

                // Open elevator doors
                yield return StartCoroutine(OpenDoors());
                isDoorOpen = true;
            }
            
        }

        // Elevator is no longer moving
        isMoving = false;
    }

    // TODO: If elevator buttons pressed, level is parented to floor node of button pressed
    // TODO: if elevatorCall button pressed, elevator goes to floor node of button pressed, if floorCounter is not 1

    // DONE // TODO: Coroutine queue for elevator calls to prevent multiple calls at once

    // TODO: GoToFloor: 
}
