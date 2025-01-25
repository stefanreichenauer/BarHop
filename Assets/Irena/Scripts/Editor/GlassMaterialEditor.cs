using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GlassMaterialProperties))]
public class GlassMaterialEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Assign Material Properties"))
        {
            GlassMaterialProperties glassProperties = (GlassMaterialProperties)target;
            glassProperties.AssignProperties();
        }
    }
}
