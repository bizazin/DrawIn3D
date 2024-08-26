using System.Collections.Generic;
using UnityEngine;
using static DrawIn3D.InputManager;

namespace DrawIn3D
{
    [RequireComponent(typeof(HitPointers))]
    public abstract class Pointer : MonoBehaviour
    {
        protected HitPointers _cachedHitPointers;

        private readonly List<Finger> _fingers = new();

        protected virtual void OnEnable() => _cachedHitPointers = GetComponent<HitPointers>();

        protected bool GetFinger(int index, Vector2 position, float pressure, out Finger finger)
        {
            foreach (var f in _fingers)
            {
                finger = f;

                if (finger.Index != index)
                    continue;
                
                StepFinger(finger, position, pressure);

                return false;
            }

            finger = new Finger();

            _fingers.Add(finger);

            InitFinger(finger, index, position, pressure);

            return true;
        }

        protected void TryNullFinger(int index)
        {
            for (var i = 0; i < _fingers.Count; i++)
            {
                var finger = _fingers[i];

                if (finger.Index != index)
                    continue;
                
                _cachedHitPointers.BreakFinger(finger);

                _fingers.RemoveAt(i);

                return;
            }
        }

        private static void InitFinger(Finger finger, int index, Vector2 screenPosition, float pressure)
        {
            finger.Index = index;

            PointOverGui(screenPosition);

            finger.ScreenPosition = screenPosition;
        }

        private static void StepFinger(Finger finger, Vector2 screenPosition, float pressure)
        {
            finger.ScreenPosition = screenPosition;
        }
    }
}