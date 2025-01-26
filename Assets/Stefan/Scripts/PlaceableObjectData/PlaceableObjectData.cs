using UnityEngine;

public enum RotationAxis
{
    X,
    Y,
    Z
}

[CreateAssetMenu(fileName = "PlaceableObjectData", menuName = "Scriptable Objects/PlaceableObjectData")]
public class PlaceableObjectData : ScriptableObject
{
    public Sprite Sprite;
    public GameObject Prefab;
    public Vector3 Scaling = Vector3.one;
    public RotationAxis RotationAxis = RotationAxis.Z;

}
