using UnityEngine;

namespace DrawIn3D
{
    public static class Common
    {
        private static Mesh _quadMesh;
        private static bool _quadMeshSet;

        private static readonly int _coord = Shader.PropertyToID("_Coord");

        public static RenderTexture GetRenderTexture(RenderTexture other) =>
            GetRenderTexture(other.descriptor, other);

        public static RenderTexture GetRenderTexture(RenderTextureDescriptor desc, RenderTexture other)
        {
            var renderTexture = GetRenderTexture(desc);

            renderTexture.filterMode = other.filterMode;
            renderTexture.anisoLevel = other.anisoLevel;
            renderTexture.wrapModeU = other.wrapModeU;
            renderTexture.wrapModeV = other.wrapModeV;

            return renderTexture;
        }

        public static RenderTexture GetRenderTexture(RenderTextureDescriptor desc) =>
            GetRenderTexture(desc, QualitySettings.activeColorSpace == ColorSpace.Gamma);

        private static RenderTexture GetRenderTexture(RenderTextureDescriptor desc, bool sRGB)
        {
            desc.sRGB = sRGB;

            return RenderTextureManager.GetTemporary(desc);
        }

        public static RenderTexture ReleaseRenderTexture(RenderTexture renderTexture) =>
            RenderTextureManager.ReleaseTemporary(renderTexture);

        public static Mesh GetQuadMesh()
        {
            if (_quadMeshSet)
                return _quadMesh;

            var gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);

            _quadMeshSet = true;
            _quadMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;

            Object.DestroyImmediate(gameObject);

            return _quadMesh;
        }

        public static Texture2D GetReadableCopy(Texture texture, TextureFormat format = TextureFormat.ARGB32,
            bool mipMaps = false, int width = 0, int height = 0)
        {
            if (width <= 0) 
                width = texture.width;

            if (height <= 0) 
                height = texture.height;

            var desc = new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32, 0);
            var renderTexture = GetRenderTexture(desc, true);

            var newTexture = new Texture2D(width, height, format, mipMaps, false);

            Helper.BeginActive(renderTexture);
            Graphics.Blit(texture, renderTexture);

            newTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            Helper.EndActive();

            ReleaseRenderTexture(renderTexture);

            newTexture.Apply();

            return newTexture;
        }

        public static void SaveBytes(string saveName, byte[] data, bool save = true)
        {
            var base64 = default(string);

            if (data != null) 
                base64 = System.Convert.ToBase64String(data);

            PlayerPrefs.SetString(saveName, base64);

            if (save) 
                PlayerPrefs.Save();
        }

        public static byte[] LoadBytes(string saveName)
        {
            var base64 = PlayerPrefs.GetString(saveName);

            return string.IsNullOrEmpty(base64) == false ? System.Convert.FromBase64String(base64) : null;
        }

        public static void Blit(RenderTexture renderTexture, Material material, int pass)
        {
            Helper.BeginActive(renderTexture);

            Draw(material, pass);

            Helper.EndActive();
        }

        public static void Draw(Material material, int pass, Mesh mesh, Matrix4x4 matrix, int subMesh)
        {
            material.SetVector(_coord, new Vector4(1, 0, 0, 0));

            if (material.SetPass(pass)) 
                Graphics.DrawMeshNow(mesh, matrix, subMesh);
        }

        public static Material BuildMaterial(string shaderName, string keyword = null)
        {
            var shader = LoadShader(shaderName);
            var material = BuildMaterial(shader);

            material.name = shaderName + keyword;

            if (string.IsNullOrEmpty(keyword) == false) 
                material.EnableKeyword(keyword);

            return material;
        }

        private static void Draw(Material material, int pass)
        {
            if (material.SetPass(pass)) 
                Graphics.DrawMeshNow(GetQuadMesh(), Matrix4x4.identity, 0);
        }

        private static Shader LoadShader(string shaderName)
        {
            var shader = Shader.Find(shaderName);

            if (shader == null)
                throw new System.Exception("Failed to find shader called: " + shaderName);

            return shader;
        }

        private static Material BuildMaterial(Shader shader)
        {
            var material = new Material(shader);
#if UNITY_EDITOR
            material.hideFlags = HideFlags.DontSave;
#endif
            return material;
        }
    }
}