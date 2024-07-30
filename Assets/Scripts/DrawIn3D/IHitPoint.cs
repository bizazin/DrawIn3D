using UnityEngine;

namespace DrawIn3D
{
	public interface IHitPoint : IHit
	{
		void HandleHitPoint(bool preview, int priority, Vector3 position, Quaternion rotation);
	}
}