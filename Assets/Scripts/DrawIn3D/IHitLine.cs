using UnityEngine;

namespace DrawIn3D
{
	public interface IHitLine : IHit
	{
		void HandleHitLine(bool preview, int priority, Vector3 position, Vector3 endPosition, Quaternion rotation);
	}
}