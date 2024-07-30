using UnityEngine;

namespace DrawIn3D
{
    [RequireComponent(typeof(HitPointers))]
    public class PointerMouse : Pointer
    {
        private const int PreviewFingerIndex = -1;
        private const int PaintFingerIndex = 1;

        private bool _oldHeld;

        protected virtual void Update()
        {
            InputManager.Finger finger;

            var newHeld = Input.GetKeyIsHeld(KeyCode.Mouse0);
            var enablePaint = newHeld || _oldHeld;
            var enablePreview = enablePaint == false;

            if (enablePreview)
            {
                GetFinger(PreviewFingerIndex, Input.GetMousePosition(), 1.0f, out finger);

                _cachedHitPointers.HandleFingerUpdate(finger, false, false);
            }

            if (enablePaint)
            {
                var down = GetFinger(PaintFingerIndex, Input.GetMousePosition(), 1.0f, out finger);

                _cachedHitPointers.HandleFingerUpdate(finger, down, newHeld == false);
            }

            if (enablePreview == false)
                TryNullFinger(PreviewFingerIndex);

            if (enablePaint == false)
                TryNullFinger(PaintFingerIndex);

            _oldHeld = newHeld;
        }
    }
}