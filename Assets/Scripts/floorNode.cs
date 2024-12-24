using UnityEngine;

public class floorNode : MonoBehaviour
{
    
    public int floorNumber; // Defines floor number
    private int floorCalled; // Defines floor called by elevatorButtons.OnButtonPressed()

    public void OnFloorCalled(int floor) // Gets the floor number from the button pressed in elevatorButtons.OnButtonPressed()
    {
        floorCalled = floor;
        Debug.Log($"Floor Called:  {floorCalled}");
    }
    
}
