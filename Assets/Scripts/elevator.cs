using System.Collections;
using UnityEngine;

public class elevator : MonoBehaviour
{
    
    [SerializeField] private GameObject elevatorBase; //Defines elevator base
    [SerializeField] private GameObject elevatorDoorLeft; //Defines left elevator door
    [SerializeField] private GameObject elevatorDoorRight; //Defines right elevator door
    [SerializeField] private GameObject doorLeftClosed; //Defines left door closed node
    [SerializeField] private GameObject doorRightClosed; //Defines right door closed node
    [SerializeField] private GameObject doorLeftOpen; //Defines left door open node
    [SerializeField] private GameObject doorRightOpen; //Defines right door open node

    public float doorSpeed = 0.5f;

    public float elevatorSpeed = 2f;

    
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
            }
            
        }
    }

}
