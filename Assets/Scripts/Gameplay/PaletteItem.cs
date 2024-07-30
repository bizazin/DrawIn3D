using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class PaletteItem : MonoBehaviour
    {
        [SerializeField] private EColor _color;
        [SerializeField] private Button _button;
        [SerializeField] private BrushSettingsDatabase _brushSettingsDatabase;
        
        public Button Button => _button;
        public EColor Color => _color;

        private void Awake() => 
            _button.image.color = _brushSettingsDatabase.GetColorByName(_color);

        public void SetActive(bool isActive) => _button.interactable = !isActive;
        
        private void OnValidate() => 
            gameObject.name = _color.ToString();
    }
}