using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Button = UnityEngine.UI.Button;

namespace App.Helpers
{
	public class BoingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField, Range(0, 100f)] public float scalePercent = 5f;
		[SerializeField, Range(0.1f, 1)] public float timeAnimation = 0.2f;
		[SerializeField] private AnimationCurve animationCurve = new AnimationCurve();

		private bool _isFirstClick;
		private Vector3 _initialScale;
		private Button _myButton;
		private bool _isWork;

		private void OnValidate()
		{
			if (animationCurve.length == 0)
				animationCurve = new AnimationCurve(new Keyframe(0, 0, 0, 2),
					new Keyframe(1, 1, 0, 0));
		}

		private void Start()
		{
			_myButton = GetComponent<Button>();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (_myButton)
				if (!_myButton.interactable)
					return;

			if (!_isFirstClick)
			{
				_isFirstClick = true;
				_initialScale = transform.localScale;
			}

			StopAllCoroutines();

			_isWork = true;
			var myTransform = transform;
			myTransform.localScale = _initialScale;
			var target = _initialScale + (_initialScale * (scalePercent / 100f));
			StartCoroutine(_Scale(myTransform, target, 0, timeAnimation / 2, animationCurve));
			
			AudioManager.Instance.Play(ClipType.Click);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			StopAllCoroutines();
			if (_isWork)
				StartCoroutine(_Scale(transform, _initialScale, 0, timeAnimation / 2, animationCurve));
			
			_isWork = false;
		}

		private void OnDisable()
		{
			StopAllCoroutines();
		}

		private static IEnumerator _Scale(Transform scaleThis, Vector3 toThis, float delay, float time,
			AnimationCurve curve)
		{
			yield return new WaitForSeconds(delay);
			var passed = 0f;
			var initScale = scaleThis.localScale;
			while (passed < time)
			{
				passed += Time.deltaTime;
				var normalized = passed / time;
				var rate = curve.Evaluate(normalized);
				scaleThis.localScale = Vector3.LerpUnclamped(initScale, toThis, rate);
				yield return null;
			}
		}
	}
}