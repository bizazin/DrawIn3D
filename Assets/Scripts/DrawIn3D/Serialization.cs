using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DrawIn3D
{
    public static class Serialization
    {
        private static readonly Dictionary<int, Material> HashToMaterial = new();

        public static int TryRegister(Material material)
        {
            var hash = GetStableStringHash(material.name);

            if (HashToMaterial.TryGetValue(hash, out var value))
                throw new System.Exception("You're trying to register the " + material +
                                           " Material, but you've already registered the " + value +
                                           " Material with the same hash.");

            HashToMaterial.Add(hash, material);

            return hash;
        }

        private static int GetStableStringHash(string s) =>
            s.Aggregate(23, (current, c) => current * 31 + c);
    }
}