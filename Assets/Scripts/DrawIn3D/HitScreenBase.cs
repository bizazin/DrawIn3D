using UnityEngine;

namespace DrawIn3D
{
    public abstract class HitScreenBase : HitPointers
    {
        private readonly LayerMask _layers = Physics.DefaultRaycastLayers;

        private Camera _camera;

        private void Awake() => 
            _camera = Camera.main;

        private void DoQuery(Vector2 screenPosition, out Hit hit3D,
            out RaycastHit2D hit2D)
        {
            var ray = _camera.ScreenPointToRay(screenPosition);
            hit2D = Physics2D.GetRayIntersection(ray, float.PositiveInfinity, _layers);

            Physics.Raycast(ray, out var hit, float.PositiveInfinity, _layers);

            hit3D = new Hit(hit);
        }

        protected void PaintAt(PointConnector connector, Vector2 screenPosition, bool preview, object owner)
        {
            DoQuery(screenPosition, out var hit3D, out var hit2D);

            var valid2D = hit2D.distance > 0.0f;
            var valid3D = hit3D.Distance > 0.0f;

            if (valid3D && (valid2D == false || hit3D.Distance < hit2D.distance))
            {
                CalcHitData(hit3D.Position, hit3D.Normal, out var finalPosition,
                    out var finalRotation);

                connector.SubmitPoint(gameObject, preview, 0, finalPosition, finalRotation,
                    owner);

                return;
            }

            connector?.BreakHits(owner);
        }

        private static void CalcHitData(Vector3 hitPoint, Vector3 hitNormal,
            out Vector3 finalPosition, out Quaternion finalRotation)
        {
            finalPosition = hitPoint;
            finalRotation = Quaternion.identity;

            var finalUp = Vector3.up;

            finalRotation = Quaternion.LookRotation(-hitNormal, finalUp);
        }
    }
}