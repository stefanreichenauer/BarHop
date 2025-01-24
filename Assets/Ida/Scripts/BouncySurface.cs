using NUnit.Framework.Constraints;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BouncySurface : MonoBehaviour
{
    [SerializeField] private float bounciness;
    private Rigidbody otherRigidbody;
    private void OnCollisionExit(Collision collision)
    {
        if (otherRigidbody != null)
        {
            otherRigidbody.linearVelocity *= bounciness;

        }
    }
    void OnCollisionEnter(Collision other)
    {
        otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
        

    }
}
