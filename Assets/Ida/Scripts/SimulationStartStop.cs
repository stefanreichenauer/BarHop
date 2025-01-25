using UnityEngine;

public class SimulationStartStop : MonoBehaviour
{
    [SerializeField] Rigidbody bubble;
    [SerializeField] Vector3 bubbleStartVelocity;
    private Vector3 bubbleOriginPos;
    private MeshRenderer bubbleRenderer;

    private void Start()
    {
        bubbleRenderer = bubble.GetComponent<MeshRenderer>();
        bubbleOriginPos = bubble.transform.position;
    }
    public void StartSimulation()
    {
        if (bubbleRenderer != null)
        {
            bubbleRenderer.enabled = true;
        }
        bubble.isKinematic = false;
        bubble.linearVelocity = bubbleStartVelocity;
    }

    public void StopSimulation()
    {
        if (bubbleRenderer != null)
        {
            bubbleRenderer.enabled = true;
        }
        bubble.transform.position = bubbleOriginPos;
        bubble.isKinematic = true;
        bubble.linearVelocity = new Vector3(0,0,0);
    }
}
