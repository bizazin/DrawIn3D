using System;
using System.Collections.Generic;
using Gameplay.Models;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "Databases/BrushSettingsDatabase", fileName = "BrushSettingsDatabase")]
    public class BrushSettingsDatabase : ScriptableObject
    {
        [SerializeField] private BrushSettingsVo _settings;
        [SerializeField] private ColorVo[] _colors;

        private readonly Dictionary<EColor, Color> _colorsDictionary = new();

        public BrushSettingsVo Settings => _settings;

        private void OnEnable()
        {
            _colorsDictionary.Clear();

            foreach (var colorVo in _colors) 
                _colorsDictionary.Add(colorVo.Name, colorVo.Color);
        }

        public Color GetColorByName(EColor colorName)
        {
            try
            {
                return _colorsDictionary[colorName];
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"[{nameof(BrushSettingsDatabase)}] Color with name {colorName} was not present in the dictionary. {e.StackTrace}");

            }
        }
    }
}