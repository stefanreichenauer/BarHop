using NUnit.Framework.Constraints;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BouncySurface : MonoBehaviour
{
    [SerializeField] private float bounciness;
    private Collision collision;

    private void Start()
    {
        collision = GetComponent<Collision>();
    }
    void OnCollisionEnter(Collider other)
    {
        Vector3 myNormalVector = collision.GetContact(0).normal;
        Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
        otherRigidbody.linearVelocity = Vector3.Reflect(otherRigidbody.linearVelocity, myNormalVector);

    }
}
