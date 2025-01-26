using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BubbleSoundPlayer : MonoBehaviour
{
    private enum Fastness
    {
        slow,
        middle,
        fast
    }
    [SerializeField] AudioSource slowBounceSound;
    [SerializeField] AudioSource fastBounceSound;
    [SerializeField] AudioSource middleBounceSound;

    [SerializeField] float slowVelocityMaximum;
    [SerializeField] float middleVelocityMaximum;

    private Rigidbody bubbleRigidbody;

    private void Start()
    {
        bubbleRigidbody = GetComponent<Rigidbody>();
    }
    public void playBounceSound()
    {
        
        switch(howFast())
        { 
            case Fastness.slow:
                slowBounceSound.Play();
                break;
            case Fastness.middle:
                middleBounceSound.Play();
                break;
            default:
                fastBounceSound.Play();
                break;
        }
        middleBounceSound.Play();
    }

    private Fastness howFast()
    {
        float velocityMagnitude = Vector3.Magnitude(bubbleRigidbody.linearVelocity);
        if (velocityMagnitude > middleVelocityMaximum)
        {
            return Fastness.fast;
        }
        else if (velocityMagnitude > slowVelocityMaximum)
        {
            return Fastness.middle;
        }
        else
        {
            return Fastness.slow;
        }
    }
}
