using System.Collections.Generic;
using UnityEngine;

namespace Script
{
	[CreateAssetMenu(menuName = "Create ColorPalette", fileName = "ColorPalette", order = 0)]
	public class ColorPalette : ScriptableObject
	{
		public Color wall;
		public Color snake;
		public Color tile;
	}
}