using System.Collections;
using UnityEngine;

namespace Script
{
	public class CanvasTutorial : MonoBehaviour
	{
		[SerializeField] private Canvas canvas;
		[SerializeField] private Animator animator;

		private bool _isFirst = true;

		public void Open()
		{
			if (!_isFirst)
				return;

			_isFirst = false;

			canvas.enabled = true;
			animator.enabled = true;


			IEnumerator Do()
			{
				yield return new WaitForSeconds(4);
				Close();
			}

			StartCoroutine(Do());
		}

		private void Close()
		{
			canvas.enabled = false;
			animator.enabled = false;
		}
	}
}