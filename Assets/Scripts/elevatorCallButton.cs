using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class elevatorCallButton : MonoBehaviour
{
    public float moveDistance = 0.02f;  // Defines the distance to move the button
    public float moveSpeed = 0.5f;      // Defines the speed to move the button
    public int floorNumber;             // Defines floor number
    private GameObject floorNode;       // Defines floor node
    private int elevatorLocation;       // Defines elevator current location
    private int nextFloor;              // Defines next floor the elevator is travelling to
    public bool isPressed = false;
    
    // On button press, move button inward
    public void OnButtonPressed()
    {
        isPressed = true;
        elevator elevator = GameObject.FindFirstObjectByType<elevator>();
        nextFloor = elevator.nextFloor;
        elevatorLocation = int.Parse(elevator.floorCounter.GetComponent<TextMeshPro>().text);
        Debug.Log($"(ECB) Call Button {floorNumber} was pressed");
        Debug.Log($"(ECB) Elevator is on floor {elevatorLocation}");
        StartCoroutine(MoveButton());

        // Print the floor queue
        Debug.Log($"(ECB) nextFloor: {nextFloor}");

        if (floorNumber == nextFloor)
        {
            Debug.Log($"(ECB) Floor {floorNumber} is already in the queue, returning");
            return;
        }
        
        // Find the floor node whos floor number matches the button pressed
        floorNode = GameObject.Find($"floorNode{floorNumber}");
        Debug.Log($"(ECB) Node retrieved: {floorNode}");

        // if floor node number matches the floor number
        if (elevatorLocation == floorNumber)
        {
            StartCoroutine(elevator.OpenDoors());
        }

        // if floor node number does not match the floor number
        else
        {
            StartCoroutine(elevator.GoToFloor(floorNumber, "elevatorCallButton"));
        }

        isPressed = false;
    }

    private IEnumerator MoveButton()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position + new Vector3(moveDistance, 0, 0);

        // Move to target position
        yield return StartCoroutine(MoveToPosition(targetPosition));

        // Move back to the starting position
        yield return StartCoroutine(MoveToPosition(startPosition));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.001f)
        {
            // Move smoothly towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Snap to the exact target position to avoid overshooting
        transform.position = targetPosition;
    }
    
}
