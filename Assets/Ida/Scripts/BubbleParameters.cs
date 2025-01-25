using UnityEngine;

[CreateAssetMenu(fileName = "BubbleParameters", menuName = "Scriptable Objects/BubbleParameters")]
public class BubbleParameters : ScriptableObject
{
    [SerializeField] public float mass;
    [SerializeField] public float bounciness;
}
