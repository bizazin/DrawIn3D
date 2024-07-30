using UnityEngine;

namespace DrawIn3D
{
    [RequireComponent(typeof(Renderer))]
    public abstract class MeshModel : Model
    {
        private MeshFilter _cachedFilter;
        private Mesh _preparedMesh;
        private Matrix4x4 _preparedMatrix;

        protected override void CacheRenderer()
        {
            base.CacheRenderer();

            _cachedFilter = GetComponent<MeshFilter>();
        }

        public override void GetPrepared(ref Mesh mesh, ref Matrix4x4 matrix)
        {
            TryGetPrepared();

            mesh = _preparedMesh;
            matrix = _preparedMatrix;
        }

        private void TryGetPrepared()
        {
            _preparedMesh = _cachedFilter.sharedMesh;
            _preparedMatrix = _cachedRenderer.localToWorldMatrix;
        }
    }
}