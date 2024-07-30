using System;
using DrawIn3D;
using UnityEngine;

namespace Gameplay
{
    public class Brush : MonoBehaviour
    {
        [SerializeField] private EColor _color;
        [SerializeField] private PaintSphere _paintSphere;
        [SerializeField] private BrushSettingsDatabase _brushSettingsDatabase;

        public EColor Color => _color;

        private void Awake() => 
            _paintSphere.Color = _brushSettingsDatabase.GetColorByName(_color);

        public void SetActive(bool isActive) => 
            gameObject.SetActive(isActive);

        public void Setup(float radius, float hardness)
        {
            SetRadius(radius);
            SetHardness(hardness);
        }

        public void SetRadius(float radius) => 
            _paintSphere.Radius = radius;

        public void SetHardness(float hardness) => 
            _paintSphere.Hardness = hardness;

        private void OnValidate() => 
            gameObject.name = _color.ToString();
    }
}