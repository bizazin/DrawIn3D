using UnityEngine;

namespace DrawIn3D
{
    public struct Hit
    {
        public readonly float Distance;
        public Vector3 Position;
        public Vector3 Normal;

        public Hit(RaycastHit hit)
        {
            Position = hit.point;
            Normal = hit.normal;
            Distance = hit.distance;
        }
    }
}