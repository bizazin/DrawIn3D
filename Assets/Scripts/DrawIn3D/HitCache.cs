using System.Collections.Generic;
using UnityEngine;

namespace DrawIn3D
{
    public class HitCache
    {
        private bool _cached;

        private readonly List<IHitPoint> _hitPoints = new();
        private readonly List<IHitLine> _hitLines = new();
        private static readonly List<IHit> Hits = new();

        public void InvokePoint(GameObject gameObject, bool preview, int priority, Vector3 position,
            Quaternion rotation)
        {
            if (_cached == false) 
                Cache(gameObject);

            foreach (var hitPoint in _hitPoints)
                hitPoint.HandleHitPoint(preview, priority, position, rotation);
        }

        public void InvokeLine(bool preview, int priority, Vector3 position, Vector3 endPosition,
            Quaternion rotation)
        {
            foreach (var hitLine in _hitLines)
                hitLine.HandleHitLine(preview, priority, position, endPosition, rotation);
        }

        private void Cache(GameObject gameObject)
        {
            _cached = true;

            gameObject.GetComponentsInChildren(Hits);

            _hitPoints.Clear();
            _hitLines.Clear();

            foreach (var hit in Hits)
            {
                _hitPoints.Add(hit as IHitPoint);

                _hitLines.Add(hit as IHitLine);
            }
        }
    }
}