using System.Collections;
using UnityEngine;

public class elevatorButtons : MonoBehaviour
{
    public float moveDistance = 0.02f;
    public float moveSpeed = 0.5f;
    public int floorNumber; // Defines floor number
    private GameObject floorNode; // Defines floor node

    // When button pressed, elevator goes to floor node that matches the floor number
    public void OnButtonPressed()
    {
        Debug.Log($"Button {floorNumber} was pressed");
        elevator elevator = GameObject.FindFirstObjectByType<elevator>();
        
        // Find the floor node whos floor number matches the button pressed
        floorNode = GameObject.Find($"floorNode{floorNumber}");
        Debug.Log($"Node retrieved: {floorNode}");
        StartCoroutine(elevator.GoToFloor(floorNumber));
    }

    private IEnumerator MoveButton()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position + new Vector3(0, 0, moveDistance);

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
