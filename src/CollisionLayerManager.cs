using System;
using System.Linq;
using System.Collections.Generic;
using Godot;

namespace HonedGodot
{
	public class CollisionLayerManager<T> where T:Enum
	{
		public U SetLayers<U>(U node, params T[] layers) where U:CollisionObject2D
		{
			node.CollisionLayer = ConvertLayersToUint(layers);
			return node;
		}

		public U SetMasks<U>(U node, params T[] layers) where U:CollisionObject2D
		{
			node.CollisionMask = ConvertLayersToUint(layers);
			return node;
		}

		public List<T> GetLayers(CollisionObject2D node)
		{
			return ConvertUintToLayers(node.CollisionLayer);
		}

		public List<T> GetMasks(CollisionObject2D node)
		{
			return ConvertUintToLayers(node.CollisionMask);
		}

		private uint ConvertLayersToUint(params T[] layers)
		{
			var str = "";
			var ints = layers.Select(x => 31 - (int)(object)x).ToArray();

			for (int i = 0; i < 32; i++)
			{
				if (ints.Contains(i))
					str += "1";
				else
					str += "0";
			}

			return Convert.ToUInt32(str, 2);
		}

		private List<T> ConvertUintToLayers(uint num)
		{
			var binaryString = Convert.ToString(num, 2).Reverse().ToArray();
			var layers = new List<T>();

			for (int i = 0; i < binaryString.Length; i++)
			{
				if (binaryString[i] == '1')
					layers.Add((T)(object)(i));
			}

			return layers;
		}
	}
}