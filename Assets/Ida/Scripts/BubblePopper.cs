using UnityEngine;

public class BubblePopper : MonoBehaviour
{
    private MeshRenderer bubbleRenderer;
    private Rigidbody bubbleRigidbody;
    [SerializeField] public float max_surface_time = 0.2f;
    [SerializeField] private AudioSource popSound;
    public bool is_popped;

    private BubbleAnimator bubbleAnimator = null;

    private void Start()
    {
        bubbleRenderer = GetComponent<MeshRenderer>();
        bubbleAnimator = GetComponentInChildren<BubbleAnimator>();
        bubbleRigidbody = GetComponent<Rigidbody>();
    }
    public void pop()
    {
        if (!is_popped)
        {
            bubbleRigidbody.isKinematic = true;
            bubbleRenderer.enabled = false;
            popSound.Play();
            is_popped = true;
            bubbleAnimator.PopBubble();
        }
    }
}
