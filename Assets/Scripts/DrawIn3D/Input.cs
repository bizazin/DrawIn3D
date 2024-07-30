
using UnityEngine;

namespace DrawIn3D
{
    public static class Input
    {
		public static Vector2 GetMousePosition() => UnityEngine.Input.mousePosition;

		public static bool GetKeyIsHeld(KeyCode oldKey) => UnityEngine.Input.GetKey(oldKey);
    }
}