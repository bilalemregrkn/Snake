using System.Collections.Generic;
using App.Helpers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
	public class ColorManager : MonoBehaviour
	{
		[SerializeField] private List<ColorPalette> listPalette;

		[SerializeField] private List<SpriteRenderer> walls;
		[SerializeField] private List<SpriteRenderer> tiles;
		[SerializeField] private SpriteRenderer snake;
		[SerializeField] private TrailRenderer tail;

		[SerializeField] private TextMeshProUGUI textGameOver;
		[SerializeField] private Image imageText;
		[SerializeField] private Image buttonTryAgain;

		[SerializeField] private ColorFunctions colorFunctions;

		private int _lastColorPaletteIndex = -1;

		public void RandomChange()
		{
			int random;
			do
			{
				random = Random.Range(0, listPalette.Count);
			} while (random == _lastColorPaletteIndex);

			_lastColorPaletteIndex = random;
			ChangeColor(random);
		}

		[Button]
		private void ChangeColor(int index)
		{
			var current = listPalette[index];

			foreach (var wall in walls)
			{
				// wall.color = current.wall;
				colorFunctions.ColorTransition(wall, current.wall);
			}

			textGameOver.color = current.wall;
			buttonTryAgain.color = current.wall;
			imageText.color = current.wall;


			foreach (var tile in tiles)
			{
				// tile.color = current.tile;
				colorFunctions.ColorTransition(tile, current.tile);
			}


			// snake.color = current.snake;
			colorFunctions.ColorTransition(snake, current.snake);
			tail.colorGradient = new Gradient()
			{
				alphaKeys = new[]
				{
					new GradientAlphaKey(1, 1),
					new GradientAlphaKey(1, 1)
				},
				colorKeys = new[]
				{
					new GradientColorKey(current.snake, 0),
					new GradientColorKey(current.snake, 1),
				}
			};
		}
	}
}