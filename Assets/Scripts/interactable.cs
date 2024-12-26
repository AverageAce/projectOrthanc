using UnityEngine;

public class Interactable : MonoBehaviour
{
    public void Interact()
    {
        /* Call other script functions */

        // Elevator Call Button
        elevatorCallButton elevatorCallButton = GetComponent<elevatorCallButton>();
        elevator elevator = GameObject.FindFirstObjectByType<elevator>();
        
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
