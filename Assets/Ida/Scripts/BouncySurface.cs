using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BouncySurface : MonoBehaviour
{
    [SerializeField] private Bounce bounce;

    private Rigidbody otherRigidbody;
    private float collidingBounciness = -1;
    private float bounciness;

    private void Start()
    {
        bounciness = bounce.getBounciness();
    }
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
            StopAllCoroutines();

        }
        
    }
    void OnCollisionEnter(Collision other)
    {
        otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
        BubblePopper popper = other.gameObject.GetComponent<BubblePopper>();

        ApplyBubbleParameters bubbleParameter = otherRigidbody.GetComponent<ApplyBubbleParameters>();
        if (bubbleParameter != null)
        {
            collidingBounciness = bubbleParameter.getBounciness();
            if (popper != null)
            {
                StartCoroutine(shouldBubblePop(popper));
            }
        }
        else
        {
            collidingBounciness = -1;
        }
        

    }

    private IEnumerator shouldBubblePop(BubblePopper popper)
    {
        yield return new WaitForSeconds(popper.max_surface_time);
        popper.pop();

    }
}
