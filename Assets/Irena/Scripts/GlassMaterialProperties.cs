using UnityEngine;

public class GlassMaterialProperties : MonoBehaviour
{
    public Color objectColor = Color.green;
    public Color specularColor = Color.blue;
    public Color innerGlowColor = Color.red;
    public Color rimColor = Color.white;

    [Range(0, 1)]
    public float transparency = 0.8f;
    //[Range(0, 1)]
    //public float lightCutoffDistance = 0.35f;
    //[Range(0, 1)]
    //public float lightCutoffSmoothness = 0.45f;
    //[Range(0, 1)]
    //public float viewCutoffDistance = 0.5f;
    //[Range(0, 20)]
    //public float outerRimPower = 0.8f;
    //[Range(0, 1)]
    //public float outerRimSmoothness = 0.5f;
    //[Range(0, 20)]
    //public float innerRimPower = 0.7f;
    //[Range(0, 1)]
    //public float innerRimSmoothness = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AssignProperties();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignProperties()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        MaterialPropertyBlock property_block = new MaterialPropertyBlock();
        property_block.SetColor("_Color", objectColor);
        property_block.SetColor("_inner_glow_color", innerGlowColor);
        property_block.SetColor("_specular_color", specularColor);
        property_block.SetColor("_rim_color", rimColor);
        property_block.SetFloat("_transparency", transparency);


        meshRenderer.SetPropertyBlock(property_block);
    }
}
