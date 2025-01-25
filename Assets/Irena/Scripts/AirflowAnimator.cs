using UnityEngine;

public class AirflowAnimator : MonoBehaviour
{
    public Transform airflowRotorBlade = null;
    public float rotationSpeed = 100.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        airflowRotorBlade.Rotate(Vector3.forward * -(rotationSpeed * Time.deltaTime), Space.Self);
    }
}
