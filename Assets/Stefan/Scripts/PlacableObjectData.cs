using UnityEngine;

public enum EPlacableObject
{
    Tree, Stone
}

[CreateAssetMenu]
public class PlacableObjectData: ScriptableObject
{
    public string DisplayName;
    public EPlacableObject Class;
    public Sprite PortraitImage;

}
