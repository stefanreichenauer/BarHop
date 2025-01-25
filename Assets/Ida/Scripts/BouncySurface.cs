using NUnit.Framework.Constraints;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BouncySurface : MonoBehaviour
{
    [SerializeField] private float bounciness;
    private Rigidbody otherRigidbody;
    private float collidingBounciness = -1;
    private void OnCollisionExit(Collision collision)
    {
        if (otherRigidbody != null)
        {
            if (collidingBounciness != -1)
            {
                otherRigidbody.linearVelocity = otherRigidbody.linearVelocity * bounciness * collidingBounciness;
            }
            else 
            {
                otherRigidbody.linearVelocity *= bounciness;
            }
            

        }
        
    }
    void OnCollisionEnter(Collision other)
    {
        otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
        ApplyBubbleParameters para = otherRigidbody.GetComponent<ApplyBubbleParameters>();
        if (para != null)
        {
            collidingBounciness = para.getBounciness();
        }
        else
        {
            collidingBounciness = -1;
        }
        

    }
}
