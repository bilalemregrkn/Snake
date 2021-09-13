using System.Collections;
using UnityEngine;

namespace Script
{
	public class EnemyController : MonoBehaviour
	{
		[SerializeField] private float maxTime;
		[SerializeField] private Vector2 startPosition;

		public void MyStart()
		{
			transform.position = startPosition;
			gameObject.SetActive(true);
			StartCoroutine(Move());
		}

		public void Stop()
		{
			StopAllCoroutines();
			gameObject.SetActive(false);
		}


		IEnumerator Move()
		{
			var min = new Vector3(1, 1, 0);
			var max = new Vector3(GameManager.Instance.MapSize.x, GameManager.Instance.MapSize.y, 0);
			var maxDistance = Vector3.Distance(max, min);

			while (true)
			{
				var size = GameManager.Instance.MapSize;
				Vector3 target;
				int count = 0;
				do
				{
					var x = Random.Range(1, size.x - 1);
					var y = Random.Range(1, size.y - 1);

					target = new Vector3(Mathf.RoundToInt(x), Mathf.RoundToInt(y), 0);
					count++;
				} while (!GameManager.Instance.IsEmpty(target) && count < 100);

				var distance = Vector3.Distance(transform.position, target);

				var nowSpeed = Mathf.Lerp(.1f, maxTime, distance / maxDistance);

				yield return MoveAnimation(transform, target, nowSpeed);
			}
			// ReSharper disable once IteratorNeverReturns
		}

		private IEnumerator MoveAnimation(Transform current, Vector3 target, float time)
		{
			float passed = 0;
			var init = current.position;
			while (passed < time)
			{
				passed += Time.deltaTime;
				var position = Vector3.Lerp(init, target, passed / time);
				current.position = position;
				yield return null;
			}
		}
	}
}