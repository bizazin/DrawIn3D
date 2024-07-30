using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class ColorPalette : MonoBehaviour
    {
        [SerializeField] private PaletteItem[] _items;

        private readonly Dictionary<EColor, PaletteItem> _itemsDictionary = new();
        private PaletteItem _activeItem;

        public IEnumerable<PaletteItem> Items => _items;

        private void OnEnable()
        {
            _itemsDictionary.Clear();
            foreach (var item in _items) 
                _itemsDictionary.Add(item.Color, item);
        }
        
        private void Awake()
        {
            foreach (var item in _items) 
                item.SetActive(false);
        }


        public void ChooseColor(EColor color)
        {
            _activeItem?.SetActive(false);
            try
            {
                var paletteItem = _itemsDictionary[color];
            
                paletteItem.SetActive(true);
            
                _activeItem = paletteItem;
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"[{nameof(ColorPalette)}] Palette item with color {color} was not present in the dictionary. {e.StackTrace}");
            }
        }
    }
}