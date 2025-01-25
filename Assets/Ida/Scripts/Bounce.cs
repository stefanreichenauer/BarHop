using UnityEngine;

[CreateAssetMenu(fileName = "Bounce", menuName = "Scriptable Objects/Bounce")]
public class Bounce : ScriptableObject
{
    [SerializeField] private float bounciness;

    public float getBounciness()
    {
        return bounciness;
    }
}
