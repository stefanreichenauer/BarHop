using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Airflow : MonoBehaviour
{
    [SerializeField] private Vector2 airflowVector;
    private Vector3 airflowVector3D;
    private Rigidbody otherRigidbody;

    private void Start()
    {
        airflowVector3D = new Vector3(airflowVector.x, airflowVector.y, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (otherRigidbody != null)
        {

            Debug.LogWarning("Only one object can be affected by the airflow at a time!");
        }
        else
        {
            Debug.Log("Enter airflow");
            Rigidbody enteredRigidbody = other.GetComponent<Rigidbody>();
            if (enteredRigidbody != null && !enteredRigidbody.isKinematic)
            {
                otherRigidbody = enteredRigidbody;
            }
            else
            {
                Debug.Log("Object without dynamic rigidbody entered!");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (otherRigidbody!=null)
        {
            
            otherRigidbody.linearVelocity += airflowVector3D;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        otherRigidbody = null;
    }
}
