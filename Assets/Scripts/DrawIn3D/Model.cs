using System.Collections.Generic;
using UnityEngine;

namespace DrawIn3D
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public abstract class Model : MonoBehaviour
    {
        protected Renderer _cachedRenderer;
        private bool _cachedRendererSet;

        private LinkedListNode<Model> _instancesNode;

        private static readonly List<Material> TempMaterials = new();
        private static readonly List<Model> TempModels = new();
        private static readonly LinkedList<Model> Instances = new();

        private static MaterialPropertyBlock _properties;

        private const string Maintex = "_MainTex";
        
        private Renderer CachedRenderer
        {
            get
            {
                if (_cachedRendererSet == false) 
                    CacheRenderer();

                return _cachedRenderer;
            }
        }

        public abstract List<PaintableTexture> FindPaintableTextures();
        public abstract void GetPrepared(ref Mesh mesh, ref Matrix4x4 matrix);

        public static List<Model> FindOverlap()
        {
            TempModels.Clear();

            foreach (var instance in Instances) 
                TempModels.Add(instance);

            return TempModels;
        }

        public void ApplyTexture(Texture texture)
        {
            _properties ??= new MaterialPropertyBlock();
            _cachedRenderer.GetPropertyBlock(_properties, 0);

            _properties.SetTexture(Maintex, texture);

            _cachedRenderer.SetPropertyBlock(_properties, 0);
        }

        public Texture GetExistingTexture()
        {
            CachedRenderer.GetSharedMaterials(TempMaterials);

            var tempMaterial = TempMaterials[0];

            return tempMaterial.GetTexture(Maintex);
        }

        protected virtual void OnEnable()
        {
            _instancesNode = Instances.AddLast(this);

            _cachedRenderer = GetComponent<Renderer>();
        }

        protected virtual void OnDisable()
        {
            Instances.Remove(_instancesNode);
            _instancesNode = null;
        }

        protected virtual void CacheRenderer()
        {
            _cachedRenderer = GetComponent<Renderer>();
            _cachedRendererSet = true;
        }
    }
}