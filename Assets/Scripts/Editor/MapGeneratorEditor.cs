using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GenerationMap))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GenerationMap mapGen = (GenerationMap)target;
        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }
        if (GUILayout.Button("Generate")) mapGen.GenerateMap();
    }
}
