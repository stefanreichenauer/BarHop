using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Airflow : MonoBehaviour
{

    [SerializeField] private PlaneDefiner planeDefiner;
    [SerializeField] private float airStrength;

    private Vector2 airflowVector;
    private Rigidbody otherRigidbody;
    private Plane propellerPlane;
    private void Start()
    {
        propellerPlane = planeDefiner.getPlane();
        airflowVector = planeDefiner.getNormal()*airStrength;
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
            if (enteredRigidbody != null && other.gameObject.CompareTag("Bubble"))
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
        if (otherRigidbody!=null && !otherRigidbody.isKinematic)
        {
            float distance_parameter = Mathf.Abs(propellerPlane.GetDistanceToPoint(other.transform.position));
            if(distance_parameter == 0)
            {
                distance_parameter = 1;
            }
            float velocityX = otherRigidbody.linearVelocity.x;
            float velocityY = otherRigidbody.linearVelocity.y;

            velocityX += airflowVector.x;

            velocityY += airflowVector.y;
            otherRigidbody.linearVelocity = new Vector3(velocityX, velocityY, otherRigidbody.linearVelocity.z);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        otherRigidbody = null;
    }
}
