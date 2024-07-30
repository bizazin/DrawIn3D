using UnityEngine;

namespace DrawIn3D
{
    public class CommandReplace : Command
    {
        private HashedTexture _texture;
        private Color _color;

        private static readonly CommandReplace Instance = new();

        private static readonly Material CachedMaterial;
        private static readonly int CachedMaterialHash;

        private static int _Texture = Shader.PropertyToID("_Texture");
        private static int _Color = Shader.PropertyToID("_Color");

        static CommandReplace() => 
            BuildMaterial(ref CachedMaterial, ref CachedMaterialHash, "Replace");

        public static void Blit(RenderTexture renderTexture, Texture texture, Color tint)
        {
            var material = Instance.SetMaterial(texture, tint);

            Instance.Apply(material);

            Common.Blit(renderTexture, material, Instance.Pass);
        }

        public override bool RequireMesh => false;

        public override void Apply(Material material)
        {
            base.Apply(material);

            material.SetTexture(_Texture, _texture);
            material.SetColor(_Color, Helper.ToLinear(_color));
        }
        
        private Material SetMaterial(Texture texture, Color color)
        {
            Material = new HashedMaterial(CachedMaterial, CachedMaterialHash);
            _texture = texture;
            _color = color;

            return CachedMaterial;
        }
    }
}