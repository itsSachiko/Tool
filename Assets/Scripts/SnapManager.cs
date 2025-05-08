using UnityEngine;

public static class SnapManager
{
    public static Vector3 GetSnappedPosition(Vector3 desiredPosition)
    {
        float snapRange = 3f;
        Collider[] nearby = Physics.OverlapSphere(desiredPosition, snapRange);

        float closestDist = float.MaxValue;
        Vector3 closestPoint = desiredPosition;

        foreach (var col in nearby)
        {
            if (!col.GetComponent<PlacedModule>()) continue;

            Vector3 point = col.ClosestPoint(desiredPosition);
            float dist = Vector3.Distance(point, desiredPosition);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestPoint = point;
            }
        }

        return closestPoint;
    }
}
