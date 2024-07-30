using System.Collections.Generic;
using UnityEngine;

namespace DrawIn3D
{
    public class HitScreen : HitScreenBase
    {
        private sealed class Link : InputManager.Link
        {
            private int _state;
            private Vector2 _screenOld;

            public void Move(Vector2 screenNew)
            {
                if (_state == 0)
                {
                    _screenOld = screenNew;
                    _state = 1;
                }
                else if (TryMove(screenNew) || _state == 2)
                    _state += 1;
            }

            private bool TryMove(Vector2 screenNew)
            {
                const float threshold = 2.0f;
                var distance = Vector2.Distance(_screenOld, screenNew);

                if (distance < threshold)
                    return false;

                _screenOld = Vector2.MoveTowards(_screenOld, screenNew, distance - threshold * 0.5f);

                return true;
            }

            public void Clear()
            {
                _state = 0;
                _screenOld = Vector2.zero;
            }
        }

        private readonly PointConnector _connector = new();

        private List<Link> _links = new();

        public override void BreakFinger(InputManager.Finger finger)
        {
            var link = InputManager.Link.Find(_links, finger);

            if (link != null)
                _connector.BreakHits(link);
        }

        public override void HandleFingerUpdate(InputManager.Finger finger, bool down, bool up)
        {
            var link = InputManager.Link.Find(_links, finger);

            if (finger.Index < 0)
            {
                if (InputManager.PointOverGui(finger.ScreenPosition))
                {
                    _connector.BreakHits(link);

                    return;
                }
            }
            else
            {
                if (down)
                {
                    if (InputManager.PointOverGui(finger.ScreenPosition))
                    {
                        _connector.BreakHits(link);

                        return;
                    }
                }
                else if (link == null)
                    return;
            }

            link ??= InputManager.Link.Create(ref _links, finger);

            link.Move(finger.ScreenPosition);

            if (finger.Index < 0)
            {
                RecordAndPaintAt(finger.ScreenPosition, true, link);

                return;
            }

            PaintEvery(link);

            base.HandleFingerUpdate(finger, down, up);
        }

        protected virtual void OnEnable()
        {
            foreach (var link in _links)
                link.Clear();

            _connector.ResetConnections();
        }
        
        protected override void HandleFingerUp(InputManager.Finger finger)
        {
            var link = InputManager.Link.Find(_links, finger);

            _connector.BreakHits(link);

            link.Clear();
        }

        private void PaintEvery(InputManager.Link link) =>
            RecordAndPaintAt(link.Finger.ScreenPosition, false, link);

        private void RecordAndPaintAt(Vector2 screenNew, bool preview, object owner) =>
            PaintAt(_connector, screenNew, preview, owner);
    }
}