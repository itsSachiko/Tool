using UnityEngine;

[CreateAssetMenu(fileName = "ModularBuildSettings", menuName = "Tools/Modular Build Settings")]
public class ModularBuildSettings : ScriptableObject
{
    public Material validMaterial;
    public Material invalidMaterial;

    private static ModularBuildSettings _instance;

    public static ModularBuildSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<ModularBuildSettings>("ModularBuildSettings");
            }
            return _instance;
        }
    }
}
