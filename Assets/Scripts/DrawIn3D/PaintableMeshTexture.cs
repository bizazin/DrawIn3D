using UnityEngine;

namespace DrawIn3D
{
    public class PaintableMeshTexture : PaintableTexture
    {
        protected override void ApplyTexture(Texture texture)
        {
            var parent = GetComponentInParent<PaintableMesh>();
            
            parent.ApplyTexture(texture);
        }
    }
}