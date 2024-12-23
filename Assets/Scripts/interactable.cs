using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Interact()
    {
        Debug.Log($"{gameObject.name} was interacted with!");

        // Call other script functions
        // Elevator Call Button
        elevatorCallButton elevatorCallButton = GetComponent<elevatorCallButton>();
        elevator elevator = GameObject.FindObjectOfType<elevator>();
        Debug.Log($"elevatorCallButton: {elevatorCallButton}");
        Debug.Log($"elevator: {elevator}");
        if (elevatorCallButton != null)
        {
            elevatorCallButton.OnButtonPressed();
            elevator.OpenDoors();
        }

        // Elevator floor buttons
        elevatorButtons elevatorButtons = GetComponent<elevatorButtons>();
        if (elevatorButtons != null)
        {
            elevatorButtons.OnButtonPressed();
        }
    }
}
