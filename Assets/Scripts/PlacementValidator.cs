using UnityEngine;

public static class PlacementValidator
{
    public static bool IsPositionValid(Vector3 position, GameObject ghost)
    {
        if (!Physics.Raycast(position + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 10f))
            return false;

        string belowTag = hit.collider.gameObject.tag;
        FurnitureType currentType = GetTypeFromName(ghost.name);

        switch (currentType)
        {
            case FurnitureType.Floor:
                return belowTag != "Roof";
            case FurnitureType.Wall:
                return belowTag != "Roof";
            case FurnitureType.Roof:
                return belowTag != "Floor";
            default:
                return false;
        }
    }

    static FurnitureType GetTypeFromName(string name)
    {
        if (name.Contains("Wall")) return FurnitureType.Wall;
        if (name.Contains("Floor")) return FurnitureType.Floor;
        if (name.Contains("Roof")) return FurnitureType.Roof;
        return FurnitureType.Floor;
    }
}
