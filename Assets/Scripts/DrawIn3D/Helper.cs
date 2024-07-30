using System.Collections.Generic;
using UnityEngine;

namespace DrawIn3D
{
	public static class Helper
	{
		private static readonly Stack<RenderTexture> Actives = new();
		
		public static Color ToLinear(Color gamma) => 
			gamma.linear;

		public static void BeginActive(RenderTexture renderTexture)
		{
			Actives.Push(RenderTexture.active);

			RenderTexture.active = renderTexture;
		}

		public static void EndActive() => 
			RenderTexture.active = Actives.Pop();

		public static void Destroy<T>(T o)
			where T : Object =>
			Object.Destroy(o);
	}
}