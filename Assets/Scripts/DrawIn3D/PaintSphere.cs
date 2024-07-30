using UnityEngine;

namespace DrawIn3D
{
    public class PaintSphere : MonoBehaviour, IHitPoint, IHitLine
    {
        private readonly BlendMode _blendMode = new() { Index = 0, Channels = Vector4.one };
        private readonly Vector3 _scale = Vector3.one;

        private const float Opacity = 1.0f;
        private const float TileOpacity = 3;
        private const float TileTransition = 4;

        public Color Color { get; set; } = Color.white;
        public float Radius { get; set; }
        public float Hardness { get; set; }

        public void HandleHitPoint(bool preview, int priority, Vector3 position,
            Quaternion rotation)
        {
            CommandSphere.Instance.SetState(preview, priority);
            CommandSphere.Instance.SetLocation(position);

            HandleHitCommon(rotation);

            HandleMaskCommon();

            PaintableManager.SubmitAll(CommandSphere.Instance);
        }

        public void HandleHitLine(bool preview, int priority, Vector3 position,
            Vector3 endPosition, Quaternion rotation)
        {
            CommandSphere.Instance.SetState(preview, priority);
            CommandSphere.Instance.SetLocation(position, endPosition);

            HandleHitCommon(rotation);

            HandleMaskCommon();

            PaintableManager.SubmitAll(CommandSphere.Instance);
        }

        private void HandleHitCommon(Quaternion rotation)
        {
            var finalRadius = Radius;
            var finalTileMatrix = Matrix4x4.identity;
            
            var finalSize = _scale * finalRadius;

            CommandSphere.Instance.SetShape(rotation, finalSize);

            CommandSphere.Instance.SetMaterial(_blendMode, Hardness, Color, Opacity, null,
                finalTileMatrix, TileOpacity, TileTransition);
        }

        private static void HandleMaskCommon() => 
            CommandSphere.Instance.ClearMask();
    }
}