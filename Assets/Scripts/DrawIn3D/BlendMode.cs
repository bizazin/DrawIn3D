using UnityEngine;

namespace DrawIn3D
{
    public struct BlendMode
    {
        public int Index;

        public Vector4 Channels;

        private static readonly int _channels = Shader.PropertyToID("_Channels");

        public void Apply(Material material) => material.SetVector(_channels, Channels);
        
        public static implicit operator int(BlendMode a) => 
            a.Index;
    }
}