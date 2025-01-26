using UnityEngine;

public class BubblePopper : MonoBehaviour
{
    private MeshRenderer bubbleRenderer;
    [SerializeField] public float max_surface_time = 0.2f;
    [SerializeField] private AudioSource popSound;
    private void Start()
    {
        bubbleRenderer = GetComponent<MeshRenderer>();
    }
    public void pop()
    {
        bubbleRenderer.enabled = false;
        popSound.Play();
    }
}
