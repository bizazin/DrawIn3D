using System;
using UnityEngine;

namespace Gameplay.Models
{
    [Serializable]
    public class BrushSettingsVo
    {
        [Range(.1f, .5f)] public float InitialRadius = .1f;
        [Range(2, 30)] public float InitialHardness = 5;
    }
}