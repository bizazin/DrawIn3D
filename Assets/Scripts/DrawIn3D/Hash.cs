using UnityEngine;

namespace DrawIn3D
{
	[System.Serializable]
	public struct Hash
	{
		[SerializeField] private int _v;
        
		public Hash(int newValue) => 
			_v = newValue;

		public static implicit operator int(Hash hash) => 
			hash._v;

		public static implicit operator Hash(int index) => 
			new(index);
	}
}