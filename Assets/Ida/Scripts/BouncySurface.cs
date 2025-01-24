using NUnit.Framework.Constraints;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BouncySurface : MonoBehaviour
{
    [SerializeField] private float bounciness;

    private void Start()
    {
        Vector3 vector3 = new Vector3(0, 1, 0);
        Vector3 vector31 = new Vector3(1, 0, 0);
        Debug.Log($"{Vector3.Reflect(vector3, vector31)}");
    }
    void OnCollisionEnter(Collision other)
    {
        Vector3 normalVector = other.GetContact(0).normal;
        
        Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (otherRigidbody != null)
        {
            Vector3 directionVector = Vector3.Reflect(otherRigidbody.linearVelocity, normalVector);
            if (directionVector == otherRigidbody.linearVelocity)
            {
                otherRigidbody.linearVelocity = directionVector * bounciness * (-1);
            }
            else
            {
                otherRigidbody.linearVelocity = directionVector*bounciness;
            }
        }

    }
}
