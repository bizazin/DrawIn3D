using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class BrushesManager : MonoBehaviour
    {
        private readonly Dictionary<EColor, Brush> _brushesDictionary = new();

        [SerializeField] private Brush[] _brushes;
        [SerializeField] private BrushSettingsDatabase _brushSettingsDatabase;
        
        private float _radius;
        private float _hardness;
        private Brush _activeBrush;

        private void OnEnable()
        {
            _brushesDictionary.Clear();
            foreach (var brush in _brushes)
                _brushesDictionary.Add(brush.Color, brush);
        }

        private void Awake()
        {
            _radius = _brushSettingsDatabase.Settings.InitialRadius;
            _hardness = _brushSettingsDatabase.Settings.InitialHardness;
            
            foreach (var brush in _brushes) 
                brush.SetActive(false);
        }

        public void EnableBrush(EColor paletteColor)
        {
            _activeBrush?.SetActive(false);
        
            try
            {
                var brush = _brushesDictionary[paletteColor];
            
                brush.Setup(_radius, _hardness);
                brush.SetActive(true);
            
                _activeBrush = brush;
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"[{nameof(BrushesManager)}] Brush with color {paletteColor} was not present in the dictionary. {e.StackTrace}");
            }
        }

        public void SetRadiusValue(float value)
        {
            _radius = value;
            _activeBrush.SetRadius(value);
        }

        public void SetHardnessValue(float value)
        {
            _hardness = value;
            _activeBrush.SetHardness(value);
        }
    }
}