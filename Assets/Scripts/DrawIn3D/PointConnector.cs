using System.Collections.Generic;
using UnityEngine;

namespace DrawIn3D
{
    public class PointConnector
    {
        private class Link
        {
            public object Owner;
            public Vector3 Position;
        }

        private readonly List<Link> _links = new();

        private static readonly Stack<Link> LinkPool = new();

        private readonly HitCache _hitCache = new();

        public void ResetConnections()
        {
            for (var i = _links.Count - 1; i >= 0; i--)
                LinkPool.Push(_links[i]);

            _links.Clear();
        }

        public void BreakHits(object owner)
        {
            for (var i = _links.Count - 1; i >= 0; i--)
            {
                var link = _links[i];

                if (link.Owner != owner)
                    continue;

                _links.RemoveAt(i);

                LinkPool.Push(link);

                return;
            }
        }

        public void SubmitPoint(GameObject gameObject, bool preview, int priority, Vector3 position,
            Quaternion rotation, object owner)
        {
            var link = default(Link);

            if (TryGetLink(owner, ref link))
                _hitCache.InvokeLine(preview, priority, link.Position, position, rotation);
            else
            {
                link = LinkPool.Count > 0 ? LinkPool.Pop() : new Link();

                link.Owner = owner;

                _links.Add(link);

                _hitCache.InvokePoint(gameObject, preview, priority, position, rotation);
            }

            link.Position = position;
        }
        
        private bool TryGetLink(object owner, ref Link foundLink)
        {
            for (var i = _links.Count - 1; i >= 0; i--)
            {
                var link = _links[i];

                if (link.Owner != owner)
                    continue;

                foundLink = link;

                return true;
            }

            return false;
        }
    }
}