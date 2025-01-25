using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Airflow : MonoBehaviour
{
    [SerializeField] private Vector2 airflowVector;
    [SerializeField] private Vector2 velocityReduction;
    [SerializeField] private PlaneDefiner planeDefiner;

    private Rigidbody otherRigidbody;
    private Plane propellerPlane;
    private void Start()
    {
        propellerPlane = planeDefiner.getPlane();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (otherRigidbody != null)
        {

            Debug.LogWarning("Only one object can be affected by the airflow at a time!");
        }
        else
        {

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
            float distance_parameter = Mathf.Abs(propellerPlane.GetDistanceToPoint(other.transform.position));
            if(distance_parameter == 0)
            {
                distance_parameter = 1;
            }
            float velocityX = otherRigidbody.linearVelocity.x;
            float velocityY = otherRigidbody.linearVelocity.y;
            velocityX *= (1 - (velocityReduction.x/ distance_parameter));
            velocityX += airflowVector.x;
            velocityY *= (1 - (velocityReduction.y/ distance_parameter));
            velocityY += airflowVector.y;
            otherRigidbody.linearVelocity = new Vector3(velocityX, velocityY, otherRigidbody.linearVelocity.z);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        otherRigidbody = null;
    }
}
