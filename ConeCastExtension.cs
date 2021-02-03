using System.Collections.Generic;
using UnityEngine;

public static class ConeCastExtension
{
    public static RaycastHit[] ConeCastAll(this Physics physics, Vector3 origin, float maxRadius,
        Vector3 direction, float maxDistance, float coneAngle, LayerMask layers)
    {
        RaycastHit[] sphereCastHits =
            Physics.SphereCastAll(origin - direction.normalized*maxRadius, maxRadius, direction, maxDistance, layers);
        List<RaycastHit> coneCastHitList = new List<RaycastHit>();
        if (sphereCastHits.Length > 0)
        {
            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                Vector3 hitPoint = sphereCastHits[i].point;
                Vector3 directionToHit = hitPoint - origin;
                float angleToHit = Vector3.Angle(direction, directionToHit);

                if (angleToHit < coneAngle)
                {
                    coneCastHitList.Add(sphereCastHits[i]);
                }
            }
        }

        return coneCastHitList.ToArray();
    }
}
