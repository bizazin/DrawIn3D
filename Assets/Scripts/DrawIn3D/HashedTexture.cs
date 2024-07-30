using System;
using UnityEngine;

namespace DrawIn3D
{
    [Serializable]
    public struct HashedTexture
    {
        private Texture _instance;

        public static implicit operator HashedTexture(Texture newInstance)
        {
            HashedTexture hashed;

            hashed._instance = newInstance;
            
            return hashed;
        }

        public static implicit operator Texture(HashedTexture hashed)
        {
            hashed.TryGetInstance(out var texture);

            return texture;
        }

        public bool TryGetInstance(out Texture texture)
        {
            if (_instance != null)
            {
                texture = _instance;
                return true;
            }

            texture = null;
            return false;
        }
    }
}