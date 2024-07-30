using UnityEngine;

namespace DrawIn3D
{
    public static class BlitUtil
    {
        private static Material _cachedBlit;
        private static bool _cachedBlitSet;

        private static int _Buffer = Shader.PropertyToID("_Buffer");
        private static int _BufferSize = Shader.PropertyToID("_BufferSize");

        public static void Blit(RenderTexture rendertexture, Mesh mesh, int submesh, Texture texture)
        {
            if (_cachedBlitSet == false)
            {
                _cachedBlit = Common.BuildMaterial("Blit");
                _cachedBlitSet = true;
            }

            Helper.BeginActive(rendertexture);

            _cachedBlit.SetTexture(_Buffer, texture);
            _cachedBlit.SetVector(_BufferSize, new Vector2(texture.width, texture.height));

            Common.Draw(_cachedBlit, 0, mesh, Matrix4x4.identity, submesh);

            Helper.EndActive();
        }
    }
}