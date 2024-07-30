using System.Collections.Generic;
using UnityEngine;

namespace DrawIn3D
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public class PaintableMesh : MeshModel
    {

        private readonly HashSet<PaintableMeshTexture> _paintableTextures = new();
        private static readonly List<PaintableTexture> TempPaintableTextures = new();
        private static readonly List<PaintableMeshTexture> TempPaintableMeshTextures = new();

        protected virtual void Activate() => DoActivate();

        public override List<PaintableTexture> FindPaintableTextures()
        {
            TempPaintableTextures.Clear();

            foreach (var paintableTexture in _paintableTextures)
                TempPaintableTextures.Add(paintableTexture);

            return TempPaintableTextures;
        }

        protected virtual void DoActivate()
        {
            AddPaintableTextures(transform);

            foreach (var paintableTexture in _paintableTextures)
                paintableTexture.Activate();
        }

        private void AddPaintableTextures(Component root)
        {
            root.GetComponents(TempPaintableMeshTextures);

            foreach (var paintableTexture in TempPaintableMeshTextures)
                _paintableTextures.Add(paintableTexture);

            TempPaintableMeshTextures.Clear();
        }

        protected virtual void Awake() => Activate();

        protected override void OnEnable()
        {
            base.OnEnable();
            PaintableManager.GetOrCreateInstance();
        }
    }
}