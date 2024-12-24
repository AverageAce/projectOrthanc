using System.Collections;
using UnityEngine;

public class elevatorCallButton : MonoBehaviour
{
    public float moveDistance = 0.02f;
    public float moveSpeed = 0.5f;
    
    // On interaction, move inward
    public void OnButtonPressed()
    {
        Debug.Log("OnButtonPressed called");
        StartCoroutine(MoveButton());
        elevator elevator = GameObject.FindFirstObjectByType<elevator>();
        Debug.Log($"elevator: {elevator}");
        // Open elevator doors
        StartCoroutine(elevator.OpenDoors());
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
