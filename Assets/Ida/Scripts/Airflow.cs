using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Airflow : MonoBehaviour
{
    [SerializeField] private AudioSource fanSound;
    [SerializeField] private PlaneDefiner planeDefiner;
    [SerializeField] private float airStrength;

    private Vector2 airflowVector;
    private Rigidbody otherRigidbody;
    private void Start()
    {
        
        airflowVector = planeDefiner.getNormal()*airStrength;
    }
    private void OnTriggerEnter(Collider other)
    {
        airflowVector = planeDefiner.getNormal() * airStrength;
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
                if (fanSound!=null)
                {
                    fanSound.Play();
                }
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
