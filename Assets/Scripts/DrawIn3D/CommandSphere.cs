using System.Collections.Generic;
using UnityEngine;

namespace DrawIn3D
{
    public class CommandSphere : Command
    {
        private BlendMode _blend;
        private Vector3 _position;
        private Vector3 _endPosition;
        private Vector3 _position2;
        private Vector3 _endPosition2;
        private int _extrusions;
        private bool _clip;
        private Matrix4x4 _matrix;
        private Color _color;
        private float _opacity;
        private float _hardness;
        private HashedTexture _tileTexture;
        private Matrix4x4 _tileMatrix;
        private float _tileOpacity;
        private float _tileTransition;
        private Matrix4x4 _maskMatrix;
        private HashedTexture _maskShape;
        private Vector4 _maskChannel;
        private Vector3 _maskStretch;
        private Vector2 _maskInvert;

        public static readonly CommandSphere Instance = new();
        private static readonly Stack<CommandSphere> _pool = new();

        private static readonly Material CachedSpotMaterial;
        private static readonly Material CachedLineMaterial;
        private static readonly Material CachedQuadMaterial;
        private static readonly Material CachedLineClipMaterial;
        private static readonly Material CachedQuadClipMaterial;

        private static readonly int CachedSpotMaterialHash;
        private static readonly int CachedLineMaterialHash;
        private static readonly int CachedQuadMaterialHash;
        private static readonly int CachedLineClipMaterialHash;
        private static readonly int CachedQuadClipMaterialHash;

        private static int _In3D = Shader.PropertyToID("_In3D");
        private static int _Position = Shader.PropertyToID("_Position");
        private static int _EndPosition = Shader.PropertyToID("_EndPosition");
        private static int _Position2 = Shader.PropertyToID("_Position2");
        private static int _EndPosition2 = Shader.PropertyToID("_EndPosition2");
        private static int _Matrix = Shader.PropertyToID("_Matrix");
        private static int _Color = Shader.PropertyToID("_Color");
        private static int _Opacity = Shader.PropertyToID("_Opacity");
        private static int _Hardness = Shader.PropertyToID("_Hardness");
        private static int _TileTexture = Shader.PropertyToID("_TileTexture");
        private static int _TileMatrix = Shader.PropertyToID("_TileMatrix");
        private static int _TileOpacity = Shader.PropertyToID("_TileOpacity");
        private static int _TileTransition = Shader.PropertyToID("_TileTransition");
        private static int _MaskMatrix = Shader.PropertyToID("_MaskMatrix");
        private static int _MaskTexture = Shader.PropertyToID("_MaskTexture");
        private static int _MaskChannel = Shader.PropertyToID("_MaskChannel");
        private static int _MaskStretch = Shader.PropertyToID("_MaskStretch");
        private static int _MaskInvert = Shader.PropertyToID("_MaskInvert");
        private static int _DepthMatrix = Shader.PropertyToID("_DepthMatrix");
        private static int _DepthTexture = Shader.PropertyToID("_DepthTexture");
        private static int _DepthBias = Shader.PropertyToID("_DepthBias");

        static CommandSphere()
        {
            BuildMaterial(ref CachedSpotMaterial, ref CachedSpotMaterialHash, "Sphere");
            BuildMaterial(ref CachedLineMaterial, ref CachedLineMaterialHash, "Sphere", "LINE");
            BuildMaterial(ref CachedQuadMaterial, ref CachedQuadMaterialHash, "Sphere", "QUAD");
            BuildMaterial(ref CachedLineClipMaterial, ref CachedLineClipMaterialHash, "Sphere",
                "LINE_CLIP");
            BuildMaterial(ref CachedQuadClipMaterial, ref CachedQuadClipMaterialHash, "Sphere",
                "QUAD_CLIP");
        }

        public override bool RequireMesh => true;

        public override void Apply(Material material)
        {
            base.Apply(material);

            _blend.Apply(material);

            var inv = _matrix.inverse;

            material.SetFloat(_In3D, 1.0f);

            material.SetVector(_Position, inv.MultiplyPoint(_position));
            material.SetVector(_EndPosition, inv.MultiplyPoint(_endPosition));
            material.SetVector(_Position2, inv.MultiplyPoint(_position2));
            material.SetVector(_EndPosition2, inv.MultiplyPoint(_endPosition2));
            material.SetMatrix(_Matrix, inv);
            material.SetColor(_Color, Helper.ToLinear(_color));
            material.SetFloat(_Opacity, _opacity);
            material.SetFloat(_Hardness, _hardness);
            material.SetTexture(_TileTexture, _tileTexture);
            material.SetMatrix(_TileMatrix, _tileMatrix);
            material.SetFloat(_TileOpacity, _tileOpacity);
            material.SetFloat(_TileTransition, _tileTransition);
            material.SetMatrix(_MaskMatrix, _maskMatrix);
            material.SetTexture(_MaskTexture, _maskShape);
            material.SetVector(_MaskChannel, _maskChannel);
            material.SetVector(_MaskStretch, _maskStretch);
            material.SetVector(_MaskInvert, _maskInvert);

            material.SetTexture(_DepthTexture, null);
            material.SetFloat(_DepthBias, -1.0f);
            material.SetMatrix(_DepthMatrix, Matrix4x4.identity);
        }

        public override void Pool() =>
            _pool.Push(this);

        public override Command SpawnCopy()
        {
            var command = SpawnCopy(_pool);

            command._blend = _blend;
            command._position = _position;
            command._endPosition = _endPosition;
            command._position2 = _position2;
            command._endPosition2 = _endPosition2;
            command._extrusions = _extrusions;
            command._clip = _clip;
            command._matrix = _matrix;
            command._color = _color;
            command._opacity = _opacity;
            command._hardness = _hardness;
            command._tileTexture = _tileTexture;
            command._tileMatrix = _tileMatrix;
            command._tileOpacity = _tileOpacity;
            command._tileTransition = _tileTransition;
            command._maskMatrix = _maskMatrix;
            command._maskShape = _maskShape;
            command._maskChannel = _maskChannel;
            command._maskStretch = _maskStretch;
            command._maskInvert = _maskInvert;

            return command;
        }

        public void SetLocation(Vector3 position, bool in3D = true)
        {
            _extrusions = 0;
            _clip = false;
            _position = position;
        }

        public void SetLocation(Vector3 position, Vector3 endPosition, bool in3D = true, bool clip = false)
        {
            _extrusions = 1;
            _clip = clip;
            _position = position;
            _endPosition = endPosition;
        }

        public void ClearMask()
        {
            _maskShape = null;
            _maskChannel = Vector3.one;
            _maskInvert = new Vector2(0.0f, 1.0f);
        }

        public void SetShape(Quaternion rotation, Vector3 size)
        {
            _matrix = Matrix4x4.TRS(Vector3.zero, rotation * Quaternion.Euler(0, 0, 0), size);
        }

        public void SetMaterial(BlendMode blendMode, float hardness, Color color, float opacity, Texture tileTexture,
            Matrix4x4 tileMatrix, float tileOpacity, float tileTransition)
        {
            switch (_extrusions)
            {
                case 0:
                    Material = new HashedMaterial(CachedSpotMaterial, CachedSpotMaterialHash);
                    break;
                case 1:
                    if (_clip)
                        Material = new HashedMaterial(CachedLineClipMaterial, CachedLineClipMaterialHash);
                    else
                        Material = new HashedMaterial(CachedLineMaterial, CachedLineMaterialHash);
                    break;
                case 2:
                    if (_clip)
                        Material = new HashedMaterial(CachedQuadClipMaterial, CachedQuadClipMaterialHash);
                    else
                        Material = new HashedMaterial(CachedQuadMaterial, CachedQuadMaterialHash);
                    break;
            }

            _blend = blendMode;
            Pass = blendMode;
            _hardness = hardness;
            _color = color;
            _opacity = opacity;
            _tileTexture = tileTexture;
            _tileMatrix = tileMatrix;
            _tileOpacity = tileOpacity;
            _tileTransition = tileTransition;
        }
    }
}