using UnityEngine;

public class BubblePopper : MonoBehaviour
{
    private MeshRenderer bubbleRenderer;
    [SerializeField] public float max_surface_time = 0.2f;
    [SerializeField] private AudioSource popSound;
    public bool is_popped;

    private void Start()
    {
        bubbleRenderer = GetComponent<MeshRenderer>();
    }
    public void pop()
    {
        if (!is_popped)
        {
            bubbleRenderer.enabled = false;
            popSound.Play();
            is_popped = true;
        }
    }
}
