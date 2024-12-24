using UnityEngine;

public class playerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;  // Defines interaction distance
    public LayerMask interactionLayer;      // Defines interaction layer for interactable objects
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))    // If E key is pressed
        {
            // Perform a raycast
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            // If raycast hits an object within the interaction distance and on the interaction layer
            if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer)) 
            {
                // Check if the object has an Interactable component
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    // Call the Interact method on the Interactable component
                    interactable.Interact();
                }
            }
        }
    }
}
