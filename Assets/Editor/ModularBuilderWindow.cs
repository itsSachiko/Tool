using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

public class ModularBuilderWindow : EditorWindow
{

    public static FurnitureType SelectedType = FurnitureType.Wall;

    [MenuItem("Editor/Tool")]
    public static void OpenWindow() => GetWindow<ModularBuilderWindow>();

    FurnitureType type = FurnitureType.Wall;

    private void OnGUI()
    {
        GUILayout.Label("Select the furniture", EditorStyles.boldLabel);
        type = (FurnitureType)EditorGUILayout.EnumPopup(type);

        if (GUILayout.Button("Spawn"))
        {
            ModularBuilder.SetActiveType(type);
            Debug.Log("mouse left click to spawn");
        }


        if (GUILayout.Button("Info Controls"))
        {
            Debug.Log("r to rotate the prefab");
        }
    }
}
