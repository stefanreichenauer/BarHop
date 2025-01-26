using UnityEngine;

public enum RotationAxis
{
    X_PLUS,
    X_MINUS,
    Y_PLUS,
    Y_MINUS,
    Z_PLUS,
    Z_MINUS
}


[CreateAssetMenu(fileName = "PlaceableObjectData", menuName = "Scriptable Objects/PlaceableObjectData")]
public class PlaceableObjectData : ScriptableObject
{
    public Sprite Sprite;
    public GameObject Prefab;
    public Vector3 Scaling = Vector3.one;
    public RotationAxis RotationAxis = RotationAxis.Z_PLUS;
}
