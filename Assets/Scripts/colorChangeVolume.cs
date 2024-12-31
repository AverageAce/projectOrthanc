using UnityEngine;

public class colorChangeVolume : MonoBehaviour
{
    public float distance;  // Defines the players distance from the sun
    public float maxDistance; // Defines the maximum distance the player can be from the sun
    public float minDistance = 8f; // Defines the minimum distance the player can be from the sun
    public GameObject sun;  // Defines the sun object
    public GameObject player;  // Defines the player object
    public Transform playerTransform;  // Defines the player transform component
    public Transform sunTransform;  // Defines the sun transform component
    public bool isInside = false;
    public Renderer sunRenderer;  // Defines the sun renderer component
    public Renderer coronaRenderer;  // Defines the sun renderer component
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sun = GameObject.Find("sun");
        player = GameObject.Find("Player");

        Material sunMaterial = sunRenderer.material;
        Material coronaMaterial = coronaRenderer.material;
    }

    void Update()
    {
        
        
        // If the player is inside the volume
        if (isInside)
        {
            // Calculate the distance between the player and the sun
            distance = Vector3.Distance(player.transform.position, sun.transform.position);
            distance = Mathf.Clamp(distance, minDistance, maxDistance);

            float normalizedDistance = (maxDistance - distance) / (maxDistance - minDistance);
            
            // Debug.Log(distance);
            Material sunMaterial = sunRenderer.material;
            Material coronaMaterial = coronaRenderer.material;

            sunMaterial.SetFloat("_DistanceFactor", normalizedDistance);
            coronaMaterial.SetFloat("_DistanceFactor", normalizedDistance);
        }
    }

    // This method is called when another collider enters the trigger collider attached to the object where this script is attached
    void OnTriggerEnter(Collider other)
    {
        maxDistance = Vector3.Distance(player.transform.position, sun.transform.position);
        
        // Check if the object that entered the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the volume");
            isInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the object that exited the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the volume");
            isInside = false;

            distance = maxDistance;

            Material sunMaterial = sunRenderer.material;
            Material coronaMaterial = coronaRenderer.material;

            sunMaterial.SetFloat("_Distance", distance);
            coronaMaterial.SetFloat("_Distance", distance);
        }
    }
}
