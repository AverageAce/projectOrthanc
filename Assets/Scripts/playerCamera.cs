using UnityEngine;

public class playerCamera : MonoBehaviour
{
    public new Camera camera;
    private elevator currentPlatform;
    private Vector3 lastPlatformPosition;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;

    void Start()
    {
        camera = GetComponent<Camera>();
    }
    
    void LateUpdate()
    {
        if (currentPlatform != null) // If current platform is not null
        {
            Vector3 targetPosition = camera.transform.position + currentPlatform.Movement;
            camera.transform.position = Vector3.SmoothDamp(camera.transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    public void SetCurrentPlatform(elevator platform)
    {
        currentPlatform = platform;
    }
}