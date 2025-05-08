using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ModularBuilder
{
    static GameObject ghost;
    static Quaternion rotation = Quaternion.identity;
    static Material validMat;
    static Material invalidMat;
    static GameObject[] prefabs;
    static FurnitureType activeType;

    static ModularBuilder()
    {
        SceneView.duringSceneGui += OnSceneGUI;

        LoadMaterials();

        prefabs = new GameObject[]
        {
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Wall.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Floor.prefab"),
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Roof.prefab")
        };
    }

    static void LoadMaterials()
    {
        validMat = ModularBuildSettings.Instance.validMaterial;
        invalidMat = ModularBuildSettings.Instance.invalidMaterial;

        //if (validMat == null || invalidMat == null)
        //{
        //    Debug.LogError("Valid or Invalid material not found. Check the paths.");
        //}
    }

    public static void SetActiveType(FurnitureType type)
    {
        activeType = type;
        CreateGhost();
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        if (ghost == null) return;

        Event e = Event.current;

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.R)
        {
            rotation *= Quaternion.Euler(0, 90, 0);
            ghost.transform.rotation = rotation;
            e.Use();
        }

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        Vector3 targetPosition;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPosition = SnapManager.GetSnappedPosition(hit.point);
        }
        else
        {
            Plane ground = new Plane(Vector3.up, Vector3.zero);
            if (ground.Raycast(ray, out float enter))
            {
                Vector3 point = ray.GetPoint(enter);
                targetPosition = SnapManager.GetSnappedPosition(point);
            }
            else return;
        }

        bool isFirst = GameObject.FindObjectsOfType<PlacedModule>().Length == 0;
        bool valid = isFirst || PlacementValidator.IsPositionValid(targetPosition, ghost);
        UpdateGhost(targetPosition, valid);

        if (e.type == EventType.MouseDown && e.button == 0 && valid)
        {
            GameObject newObject = PrefabUtility.InstantiatePrefab(prefabs[(int)activeType]) as GameObject;
            newObject.transform.position = targetPosition;
            newObject.transform.rotation = rotation;

            newObject.AddComponent<PlacedModule>();

            e.Use();
        }

    }


    static void CreateGhost()
    {
        if (ghost != null)
            GameObject.DestroyImmediate(ghost);

        ghost = GameObject.Instantiate(prefabs[(int)activeType]);
        ghost.name = "GhostPreview";

        foreach (var c in ghost.GetComponentsInChildren<Collider>())
            c.enabled = false;

        ApplyGhostMaterial(invalidMat);
    }

    static void UpdateGhost(Vector3 position, bool valid)
    {
        if (ghost == null) return;

        ghost.transform.position = position;
        ghost.transform.rotation = rotation;
        ApplyGhostMaterial(valid ? validMat : invalidMat);
    }

    static void ApplyGhostMaterial(Material mat)
    {
        foreach (var renderer in ghost.GetComponentsInChildren<Renderer>())
            renderer.sharedMaterial = mat;
    }

    static void PlaceObject(Vector3 targetPosition)
    {
        GameObject newObject = PrefabUtility.InstantiatePrefab(prefabs[(int)activeType]) as GameObject;
        newObject.transform.position = targetPosition;
        newObject.transform.rotation = rotation;

        newObject.AddComponent<PlacedModule>();

        GameObject.DestroyImmediate(ghost);
    }
}
