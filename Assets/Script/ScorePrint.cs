using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script
{
	public class ScorePrint : MonoBehaviour
	{
		[SerializeField] private List<PixelNumberMap> listPixelNumberMap;
		[SerializeField] private List<PixelNumberMapAdapter> listPixelNumberMapAdapters;

		private const int LengthAxis = 3;
		private const int LengthVertical = 5;

		[SerializeField] private List<GameObject> onesListTileWall;
		[SerializeField] private List<GameObject> tensListTileWall;
		[SerializeField] private List<GameObject> hundredsListTileWall;

		private List<List<GameObject>> _current = new List<List<GameObject>>();

		public List<Vector2> listExceptPosition = new List<Vector2>();

		[Button]
		public void LoadAdapter()
		{
			listPixelNumberMapAdapters = new List<PixelNumberMapAdapter>();

			foreach (var pixelNumberMap in listPixelNumberMap)
			{
				var adapter = new PixelNumberMapAdapter();

				for (int i = 0; i < LengthVertical; i++)
				{
					for (int j = 0; j < LengthAxis; j++)
					{
						var value = pixelNumberMap.CustomCellDrawing[j, i];
						adapter.data.Add(value);
					}
				}

				listPixelNumberMapAdapters.Add(adapter);
			}
		}

		private void Awake()
		{
			_current = new List<List<GameObject>>()
			{
				onesListTileWall,
				tensListTileWall,
				hundredsListTileWall
			};
		}

		private void Start()
		{
			ResetPrint();
		}

		public void ResetPrint()
		{
			listExceptPosition.Clear();
			foreach (var item in onesListTileWall)
				item.SetActive(false);
			foreach (var item in tensListTileWall)
				item.SetActive(false);
			foreach (var item in hundredsListTileWall)
				item.SetActive(false);
		}

		private List<Vector2> GetExceptList(Vector2 offset)
		{
			List<Vector2> result = new List<Vector2>();
			var size = GameManager.Instance.MapSize;
			var center = new Vector2(size.x / 2, size.y / 2);

			for (int i = 0, axis = 0; i < LengthAxis; i++, axis++)
			{
				for (int j = LengthVertical - 1, vertical = 0; j >= 0; j--, vertical++)
				{
					int positionX = Mathf.RoundToInt(center.x) + axis - 1 + Mathf.RoundToInt(offset.x);
					int positionY = Mathf.RoundToInt(center.y) + vertical - 2 + Mathf.RoundToInt(offset.y);


					result.Add(new Vector2(positionX, positionY));
				}
			}

			return result;
		}

		#region Test

		[SerializeField] private GameObject test;

		[Button]
		public void TestShowExcept()
		{
			foreach (var item in listExceptPosition)
			{
				Instantiate(test, new Vector3(item.x, item.y, 0), Quaternion.identity);
			}
		}

		#endregion

		[Button]
		public void Print(int value)
		{
			if (value < 10)
			{
				Print(value, Vector2.zero, 0);
			}
			else if (value < 100)
			{
				var ones = value % 10;
				var tens = value / 10;

				Print(ones, new Vector2(2, 0), 1);
				Print(tens, new Vector2(-2, 0), 0);
			}
			else
			{
				var ones = value % 10;
				var tens = (value % 100) / 10;
				var hundreds = value / 100;

				Print(ones, new Vector2(4, 0), 0);
				Print(tens, new Vector2(0, 0), 1);
				Print(hundreds, new Vector2(-4, 0), 2);
			}


			//Create Except List
			if (listExceptPosition.Count == 0)
			{
				var ones = GetExceptList(Vector2.zero);
				foreach (var item in ones)
					listExceptPosition.Add(item);
			}
			else if (value >= 10 && listExceptPosition.Count < 20)
			{
				listExceptPosition.Clear();
				var ones = GetExceptList(new Vector2(2, 0));
				var tens = GetExceptList(new Vector2(-2, 0));

				foreach (var item in ones)
					listExceptPosition.Add(item);
				foreach (var item in tens)
					listExceptPosition.Add(item);
			}
			else if (value >= 100 && listExceptPosition.Count < 31)
			{
				listExceptPosition.Clear();
				var ones = GetExceptList(new Vector2(4, 0));
				var tens = GetExceptList(new Vector2(0, 0));
				var hundreds = GetExceptList(new Vector2(-4, 0));

				foreach (var item in ones)
					listExceptPosition.Add(item);
				foreach (var item in tens)
					listExceptPosition.Add(item);
				foreach (var item in hundreds)
					listExceptPosition.Add(item);
			}
		}

		private void Print(int value, Vector2 offset, int index)
		{
			var listTileWall = _current[index];

			var size = GameManager.Instance.MapSize;
			var center = new Vector2(size.x / 2, size.y / 2);

			// var data = listPixelNumberMap[value];
			var dataAdapter = listPixelNumberMapAdapters[value];

			foreach (var tile in listTileWall)
				tile.SetActive(false);

			int listIndex = 0;
			for (int i = 0, axis = 0; i < LengthAxis; i++, axis++)
			{
				for (int j = LengthVertical - 1, vertical = 0; j >= 0; j--, vertical++)
				{
					// if (!data.CustomCellDrawing[i, j])
					// 	continue;

					if (!dataAdapter.GetValue(i, j))
						continue;

					var tile = listTileWall[listIndex];
					listIndex++;

					int positionX = Mathf.RoundToInt(center.x) + axis - 1 + Mathf.RoundToInt(offset.x);
					int positionY = Mathf.RoundToInt(center.y) + vertical - 2 + Mathf.RoundToInt(offset.y);

					tile.transform.position = new Vector3(positionX, positionY, 0);
					tile.SetActive(true);
				}
			}
		}
	}
}