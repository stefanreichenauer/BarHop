using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableObjectsPerLevel", menuName = "Scriptable Objects/PlaceableObjectsPerLevel")]
public class PlaceableObjectsPerLevel : ScriptableObject
{
    public PlaceableObjectData[] objects;
}
