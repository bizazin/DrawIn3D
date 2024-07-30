using DrawIn3D;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private Slider _radiusSlider;
        [SerializeField] private Slider _hardnessSlider;
        [SerializeField] private Button _clearButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private ColorPalette _colorPalette;
        [SerializeField] private BrushesManager _brushesManager;
        [SerializeField] private PaintableTexture _paintableTexture;
        [SerializeField] private BrushSettingsDatabase _brushSettingsDatabase;

        private void Awake()
        {
            foreach (var item in _colorPalette.Items) 
                item.Button.onClick.AddListener(() => OnColorClick(item.Color));
        
            _saveButton.onClick.AddListener(OnSaveButton);
            _loadButton.onClick.AddListener(OnLoadButton);
            _clearButton.onClick.AddListener(OnClearButton);
        
            _radiusSlider.onValueChanged.AddListener(OnRadiusValueChanged);
            _hardnessSlider.onValueChanged.AddListener(OnHardnessValueChanged);
        }

        private void OnDestroy()
        {
            foreach (var item in _colorPalette.Items) 
                item.Button.onClick.RemoveListener(() => OnColorClick(item.Color));
        
            _saveButton.onClick.RemoveListener(OnSaveButton);
            _loadButton.onClick.RemoveListener(OnLoadButton);
            _clearButton.onClick.RemoveListener(OnClearButton);

            _radiusSlider.value = _brushSettingsDatabase.Settings.InitialRadius;
            _hardnessSlider.value = _brushSettingsDatabase.Settings.InitialHardness;
        
            _radiusSlider.onValueChanged.RemoveListener(OnRadiusValueChanged);
            _hardnessSlider.onValueChanged.RemoveListener(OnHardnessValueChanged);
        }

        private void OnRadiusValueChanged(float value) => 
            _brushesManager.SetRadiusValue(value);

        private void OnHardnessValueChanged(float value) => 
            _brushesManager.SetHardnessValue(value);

        private void OnClearButton() => 
            _paintableTexture.Clear();

        private void OnSaveButton()
        {
            _paintableTexture.Save();
        }

        private void OnLoadButton() => 
            _paintableTexture.Load();

        private void OnColorClick(EColor paletteColor) => 
            _brushesManager.EnableBrush(paletteColor);
    }
}