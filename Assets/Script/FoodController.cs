using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script
{
	public class FoodController : MonoBehaviour
	{
		[SerializeField] private List<Sprite> listSprite;
		[SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField] private SpriteMask spriteMask;
		[SerializeField] private Animator lastChangeAnimation;

		public int score;
		[SerializeField] private bool isBonus;
		[SerializeField, Range(3, 10)] private int staySecond;

		private const int AnimLength = 2;
		private const string KeyIdle = "food-mask-idle";
		private const string KeyFlash = "food-flash";

		public void OnEat()
		{
			var sprite = listSprite[Random.Range(0, listSprite.Count)];

			spriteRenderer.sprite = sprite;
			spriteMask.sprite = sprite;

			if (isBonus)
			{
				gameObject.SetActive(false);
				StopAllCoroutines();
				GameManager.Instance.BonusLoop();
			}
		}

		public void OnSpawn()
		{
			lastChangeAnimation.Play(KeyIdle);

			IEnumerator Do()
			{
				yield return new WaitForSeconds(staySecond - AnimLength);
				lastChangeAnimation.Play(KeyFlash);
				yield return new WaitForSeconds(AnimLength);
				gameObject.SetActive(false);
				GameManager.Instance.BonusLoop();
			}

			StartCoroutine(Do());
		}
	}
}