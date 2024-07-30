using UnityEngine;

namespace DrawIn3D
{
    public static class RenderTextureManager
    {
        public static RenderTexture GetTemporary(RenderTextureDescriptor desc)
        {
            var renderTexture = RenderTexture.GetTemporary(desc);

            return renderTexture;
        }

        public static RenderTexture ReleaseTemporary(RenderTexture renderTexture)
        {
            if (renderTexture == null)
                return null;

            renderTexture.DiscardContents();

            RenderTexture.ReleaseTemporary(renderTexture);

            return null;
        }
    }
}