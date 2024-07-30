using UnityEngine;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ColorPalette _colorPalette;
        [SerializeField] private BrushesManager _brushesManager;

        private void Start()
        {
            _colorPalette.ChooseColor(0);
            _brushesManager.EnableBrush(0);
        }
    }
}