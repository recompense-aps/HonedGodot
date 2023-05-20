using Godot;
using System.Collections.Generic;

namespace HonedGodot.Extensions
{
	public static class DataStructureExtensions
	{
		public static List<T> ToList<T>(this Godot.Collections.Array array)
		{
			var list = new List<T>();

			foreach( var item in array)
				list.Add((T)item);

			return list;
		}
	}
}