using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Script
{
	[CreateAssetMenu(menuName = "Create PixelNumberMap", fileName = "PixelNumberMap", order = 0)]
	public class PixelNumberMap : SerializedScriptableObject
	{
		[TableMatrix(DrawElementMethod = "DrawCell")]
		public bool[,] CustomCellDrawing = new bool[3, 5];

		static readonly Color Active = Color.green;
		static readonly Color Inactive = Color.gray;

		private static bool DrawCell(Rect rect, bool value)
		{
			if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
			{
				value = !value;
				GUI.changed = true;
				Event.current.Use();
			}

#if UNITY_EDITOR
			EditorGUI.DrawRect(rect.Padding(1), value ? Active : Inactive);
#endif

			return value;
		}
	}

	[Serializable]
	public class PixelNumberMapAdapter
	{
		[SerializeField] public List<bool> data = new List<bool>();

		public bool GetValue(int i, int j)
		{
			var index = (j * 3) + i;
			return data[index];
		}
	}
}