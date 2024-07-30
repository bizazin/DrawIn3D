using UnityEngine;

namespace DrawIn3D
{
	public abstract class HitPointers : MonoBehaviour
	{
		public virtual void BreakFinger(InputManager.Finger finger)
		{
		}

		public virtual void HandleFingerUpdate(InputManager.Finger finger, bool down, bool up)
		{
			if (up) 
				HandleFingerUp(finger);

			PaintableManager.MarkActivelyPainting();
		}

		protected virtual void HandleFingerUp(InputManager.Finger finger)
		{
		}
	}
}