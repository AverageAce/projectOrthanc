using UnityEngine;

public class elevatorTrigger : MonoBehaviour
{
    /*
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered elevator trigger");
            // Parents player to elevator to negate jittering
            other.transform.SetParent(transform);
            Debug.Log("Player's parent: " + other.transform.parent);
        }
        else
        {
            other.transform.parent = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited elevator trigger");
            // Unparents player from elevator
            other.transform.SetParent(null);
            Debug.Log("Player's parent: " + other.transform.parent);
        }
    }
    */

}
