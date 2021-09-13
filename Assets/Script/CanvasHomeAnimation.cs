using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
	public class CanvasHomeAnimation : MonoBehaviour
	{
		[SerializeField] private List<Image> listImage;
		[SerializeField] private List<Sprite> listSprite;

		[SerializeField] private float time;

		private void Start()
		{
			StartCoroutine(Animation());
		}

		private IEnumerator Animation()
		{
			while (true)
			{
				foreach (var image in listImage)
				{
					Sprite sprite;
					do
					{
						sprite = listSprite[Random.Range(0, listSprite.Count)];
					} while (image.sprite == sprite);

					image.sprite = sprite;
					yield return new WaitForSeconds(time);
				}
			}
		}
	}
}