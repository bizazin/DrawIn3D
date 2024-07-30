using System;

namespace DrawIn3D
{
    [Serializable]
    public struct HashedModel
    {
        private Model _instance;

        public static implicit operator HashedModel(Model newInstance)
        {
            HashedModel hashed;

            hashed._instance = newInstance;

            return hashed;
        }

        public bool TryGetInstance(out Model model)
        {
            model = _instance;
            return true;
        }
    }
}