using UnityEngine;

public class sun : MonoBehaviour
{
    private GameObject sunObject;
    private GameObject coronaObject;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sunObject = GameObject.Find("sun");
        coronaObject = GameObject.Find("coronaContainer");
    }

    // Update is called once per frame
    void Update()
    {
        // Make coronaObject position the same as sunObject
        coronaObject.transform.position = sunObject.transform.position;
        // Make corona always face the camera based on its x-axis
        coronaObject.transform.LookAt(Camera.main.transform, Vector3.up);
    }
}
