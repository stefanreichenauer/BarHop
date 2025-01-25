using UnityEditor.ShaderGraph;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ApplyBubbleParameters:MonoBehaviour
{
    [SerializeField] BubbleParameters bubbleParameters;

    private void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = bubbleParameters.mass;
    }

    public float getBounciness()
    {
        return bubbleParameters.bounciness;
    }
}
