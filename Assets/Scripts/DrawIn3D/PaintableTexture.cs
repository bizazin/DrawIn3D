using System.Collections.Generic;
using UnityEngine;

namespace DrawIn3D
{
    public abstract class PaintableTexture : MonoBehaviour
    {
        [SerializeField] private string _saveName;

        private Texture _texture;
        private readonly Color _color = Color.white;
        private bool _activated;
        private RenderTexture _current;
        private RenderTexture _preview;
        private int _stateIndex;
        private Model _model;
        private bool _paintableSet;
        private Texture _oldTexture;
        private LinkedListNode<PaintableTexture> _instancesNode;

        private const RenderTextureFormat Format = RenderTextureFormat.ARGB32;
        private const int Width = 1024;
        private const int Height = 1024;

        private static System.Action<PaintableTexture> _onInstanceAdded;
        private static System.Action<PaintableTexture> _onInstanceRemoved;

        private readonly List<Command> _paintCommands = new();
        private readonly List<Command> _previewCommands = new();

        private static readonly int Buffer = Shader.PropertyToID("_Buffer");
        private static readonly int BufferSize = Shader.PropertyToID("_BufferSize");

        public static LinkedList<PaintableTexture> Textures { get; } = new();

        private bool CommandsPending => _paintCommands.Count + _previewCommands.Count > 0;


        public void Activate()
        {
            if (_activated)
                return;

            _model = GetComponentInParent<Model>();

            if (_model == null)
                return;

            _oldTexture = _model.GetExistingTexture();

            var finalTexture = _texture;


            if (finalTexture == null)
            {
                finalTexture = _oldTexture;

                _texture = _oldTexture;
            }

            var desc = new RenderTextureDescriptor(Width, Height, Format, 0)
            {
                autoGenerateMips = false
            };

            _current = Common.GetRenderTexture(desc);

            if (finalTexture != null)
                _current.filterMode = finalTexture.filterMode;

            if (finalTexture != null)
                _current.anisoLevel = finalTexture.anisoLevel;

            if (finalTexture != null)
                _current.wrapModeU = finalTexture.wrapModeU;

            if (finalTexture != null)
                _current.wrapModeV = finalTexture.wrapModeV;

            _activated = true;

            Clear(finalTexture, _color);

            ApplyTexture(_current);
        }

        public void AddCommand(Command command)
        {
            if (command.Preview)
            {
                command.Index = _previewCommands.Count;

                _previewCommands.Add(command);
            }
            else
            {
                command.Index = _paintCommands.Count;

                _paintCommands.Add(command);
            }
        }

        public void ExecuteCommands()
        {
            if (!_activated)
                return;

            var hidePreview = true;

            if (CommandsPending)
            {
                var oldActive = RenderTexture.active;

                if (_paintCommands.Count > 0)
                    ExecuteCommands(_paintCommands, _current, ref _preview);

                var swap = _preview;

                _preview = null;

                if (_previewCommands.Count > 0)
                {
                    _preview = swap;
                    swap = null;

                    if (_preview == null)
                        _preview = Common.GetRenderTexture(_current);

                    hidePreview = false;

                    _preview.DiscardContents();

                    Graphics.Blit(_current, _preview);

                    ExecuteCommands(_previewCommands, _preview, ref swap);
                }

                Common.ReleaseRenderTexture(swap);

                RenderTexture.active = oldActive;
            }

            if (hidePreview)
                _preview = Common.ReleaseRenderTexture(_preview);

            ApplyTexture(_preview != null ? _preview : _current);
        }

        public void Clear() => Clear(_texture, _color);


        protected virtual void OnEnable()
        {
            _instancesNode = Textures.AddLast(this);

            _onInstanceAdded?.Invoke(this);
        }

        protected virtual void OnDisable()
        {
            Textures.Remove(_instancesNode);
            _instancesNode = null;

            _onInstanceRemoved?.Invoke(this);
        }

        protected virtual void OnApplicationPause(bool paused)
        {
            if (!paused)
                return;

            if (!_activated)
                return;

            Save();
        }

        protected virtual void OnDestroy()
        {
            if (!_activated)
                return;

            Save();

            Common.ReleaseRenderTexture(_current);
            Common.ReleaseRenderTexture(_preview);
        }

        protected virtual void ApplyTexture(Texture texture)
        {
            if (_model != null)
                _model.ApplyTexture(texture);
        }

        private Texture2D GetReadableCopy()
        {
            Texture2D copy;

            if (_activated)
                copy = Common.GetReadableCopy(_current);
            else
            {
                var desc = new RenderTextureDescriptor(Width, Height, Format, 0);
                var temp = Common.GetRenderTexture(desc);
                var finalTexture = _texture;

                if (finalTexture == null)
                    if (_model != null)
                        finalTexture = _model.GetExistingTexture();

                CommandReplace.Blit(temp, finalTexture, _color);

                copy = Common.GetReadableCopy(temp);
            }

            return copy;
        }

        private byte[] GetPngData()
        {
            var tempTexture = GetReadableCopy();

            if (tempTexture == null)
                return null;


            var data = tempTexture.EncodeToPNG();

            Helper.Destroy(tempTexture);

            return data;
        }

        private void Clear(Texture texture, Color tint, bool updateMips = true)
        {
            if (!_activated)
                return;

            CommandReplace.Blit(_current, texture, tint);

            if (updateMips && _current.useMipMap)
                _current.GenerateMips();
        }

        private void Replace(Texture texture, Color tint)
        {
            if (texture != null)
                Resize(texture.width, texture.height, false);
            else
                Resize(Width, Height, false);

            Clear(texture, tint);
        }

        private void Resize(int width, int height, bool copyContents = true)
        {
            if (!_activated)
                return;

            if (_current.width == width && _current.height == height)
                return;

            var descriptor = _current.descriptor;

            descriptor.width = width;
            descriptor.height = height;

            var newCurrent = Common.GetRenderTexture(descriptor, _current);

            if (copyContents)
            {
                CommandReplace.Blit(newCurrent, _current, Color.white);

                if (newCurrent.useMipMap)
                    newCurrent.GenerateMips();
            }

            Common.ReleaseRenderTexture(_current);

            _current = newCurrent;
        }

        public void Save() => 
            Save(_saveName);


        private void Save(string saveName)
        {
            if (_activated && string.IsNullOrEmpty(saveName) == false)
                Common.SaveBytes(saveName, GetPngData());
        }

        public void Load() => Load(_saveName);


        private void Load(string saveName)
        {
            if (_activated)
                LoadFromData(Common.LoadBytes(saveName));
        }

        private void LoadFromData(byte[] data, bool allowResize = true)
        {
            if (data is not { Length: > 0 })
                return;

            var tempTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false, false);

            tempTexture.LoadImage(data);

            if (allowResize)
                Replace(tempTexture, Color.white);
            else
                Clear(tempTexture, Color.white);

            Helper.Destroy(tempTexture);
        }

        private static void ExecuteCommands(List<Command> commands, RenderTexture main,
            ref RenderTexture swap)
        {
            RenderTexture.active = main;

            foreach (var command in commands)
            {
                command.Model.TryGetInstance(out var commandModel);
                command.Material.TryGetInstance(out var commandMaterial);
                var preparedMesh = default(Mesh);
                var preparedMatrix = Matrix4x4.identity;
                var preparedSubmesh = 0;

                if (command.RequireMesh)
                {
                    commandModel.GetPrepared(ref preparedMesh, ref preparedMatrix);

                    preparedSubmesh = command.Submesh;
                }
                else
                    preparedMesh = Common.GetQuadMesh();

                if (swap == null)
                    swap = Common.GetRenderTexture(main);

                BlitUtil.Blit(swap, preparedMesh, preparedSubmesh, main);

                commandMaterial.SetTexture(Buffer, swap);
                commandMaterial.SetVector(BufferSize, new Vector2(swap.width, swap.height));

                command.Apply(commandMaterial);

                RenderTexture.active = main;

                Common.Draw(commandMaterial, command.Pass, preparedMesh, preparedMatrix, preparedSubmesh);

                command.Pool();
            }

            commands.Clear();
        }
    }
}