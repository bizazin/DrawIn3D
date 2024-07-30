using System;
using UnityEngine;

namespace DrawIn3D
{
    [Serializable]
    public struct HashedMaterial
    {
        private Material _instance;

        public HashedMaterial(Material newInstance, int newHash)
        {
            _instance = newInstance;
        }

        public bool TryGetInstance(out Material model)
        {
            model = _instance;

            return true;

        }
    }
}