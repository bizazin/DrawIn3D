using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrawIn3D
{
    [Serializable]
    public abstract class Command
    {
        public int Index;
        public bool Preview;
        public int Priority;
        public HashedMaterial Material;
        public int Pass;
        public HashedModel Model;
        public int Submesh;
        public HashedTexture LocalMaskTexture;
        public Vector4 LocalMaskChannel;

        private static int _LocalMaskTexture = Shader.PropertyToID("_LocalMaskTexture");
        private static int _LocalMaskChannel = Shader.PropertyToID("_LocalMaskChannel");

        public abstract bool RequireMesh { get; }

        public static void BuildMaterial(ref Material material, ref int materialHash, string path,
            string keyword = null)
        {
            material = Common.BuildMaterial(path, keyword);
            materialHash = Serialization.TryRegister(material);
        }

        public void SetState(bool preview, int priority)
        {
            Preview = preview;
            Priority = priority;
            Index = 0;
        }

        public virtual void Apply(Material material)
        {
            material.SetTexture(_LocalMaskTexture, LocalMaskTexture);
            material.SetVector(_LocalMaskChannel, LocalMaskChannel);
        }

        public virtual void Pool()
        {
        }

        public virtual Command SpawnCopy() => null;

        public virtual void Apply()
        {
            LocalMaskTexture = null;
            LocalMaskChannel = new Vector4(1, 0, 0, 0);
        }

        protected T SpawnCopy<T>(Stack<T> pool)
            where T : Command, new()
        {
            var command = pool.Count > 0 ? pool.Pop() : new T();

            command.Index = Index;
            command.Preview = Preview;
            command.Priority = Priority;
            command.Material = Material;
            command.Pass = Pass;
            command.Model = Model;
            command.Submesh = Submesh;
            command.LocalMaskTexture = LocalMaskTexture;
            command.LocalMaskChannel = LocalMaskChannel;

            return command;
        }
    }
}