using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Helpers
{
	public class ColorFunctions : MonoBehaviour
	{
		[SerializeField] private AnimationCurve defaultCurve = new AnimationCurve();

		private void OnValidate()
		{
			if (defaultCurve.length == 0)
			{
				defaultCurve = AnimationCurve.Linear(0, 0, 1, 1);
			}
		}

		public void ChangeColor(SpriteRenderer changeThis, Color toThis, float delay = 0f)
		{
			StartCoroutine(_ChangeColor(changeThis, toThis, delay));
		}

		public IEnumerator _ChangeColor(SpriteRenderer changeThis, Color toThis, float delay = 0f)
		{
			yield return new WaitForSeconds(delay);

			changeThis.color = toThis;
		}

		public void ColorTransition(SpriteRenderer changeThis, Color toThis, float time = 0.2f, float delay = 0, AnimationCurve curve = null)
		{
			curve ??= defaultCurve;
			StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, curve));
		}

		public IEnumerator _ColorTransition(SpriteRenderer changeThis, Color toThis, float delay, float time,
			AnimationCurve curve)
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;
			Color initColor = changeThis.color;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				changeThis.color = Color.LerpUnclamped(initColor, toThis, rate);
				yield return null;
			}
		}

		public IEnumerator _ColorTransition(Image changeThis, Color toThis, float delay, float time)
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;
			Color initColor = changeThis.color;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = defaultCurve.Evaluate(passed / time);

				changeThis.color = Color.LerpUnclamped(initColor, toThis, rate);
				yield return null;
			}
		}

		public void ColorTransition(Image changeThis, Color toThis, float delay, float time, AnimationCurve curve)
		{
			StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, curve));
		}

		public void ColorTransition(Image changeThis, Color toThis, float delay, float time)
		{
			StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, defaultCurve));
		}

		public IEnumerator _ColorTransition(Image changeThis, Color toThis, float delay, float time,
			AnimationCurve curve)
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;

			Color initColor = Color.white;
			if (changeThis != null) initColor = changeThis.color;


			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				if (changeThis != null) changeThis.color = Color.LerpUnclamped(initColor, toThis, rate);
				yield return null;
			}
		}

		#region SVG

		// public void ColorTransition(SVGImage changeThis, Color toThis, float delay, float time, AnimationCurve curve)
		// {
		//     StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, curve));
		// }
		//
		// public void ColorTransition(SVGImage changeThis, Color toThis, float delay, float time)
		// {
		//     StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, defaultCurve));
		// }
		//
		// public IEnumerator _ColorTransition(SVGImage changeThis, Color toThis, float delay, float time, AnimationCurve curve)
		// {
		//     yield return new WaitForSeconds(delay);
		//     float passed = 0f;
		//     float rate = 0f;
		//     Color initColor = changeThis.color;
		//
		//     while (passed < time)
		//     {
		//         passed += Time.deltaTime;
		//         rate = curve.Evaluate(passed / time);
		//
		//         changeThis.color = Color.LerpUnclamped(initColor, toThis, rate);
		//         yield return null;
		//     }
		// }
		//
		// public IEnumerator _ColorTransition(SVGImage changeThis, Color toThis, float delay, float time )
		// {
		//     yield return new WaitForSeconds(delay);
		//     float passed = 0f;
		//     float rate = 0f;
		//     Color initColor = changeThis.color;
		//
		//     while (passed < time)
		//     {
		//         passed += Time.deltaTime;
		//         rate = defaultCurve.Evaluate(passed / time);
		//
		//         changeThis.color = Color.LerpUnclamped(initColor, toThis, rate);
		//         yield return null;
		//     }
		// }

		#endregion


		public void ColorTransition(TextMeshProUGUI changeThis, Color toThis, float delay, float time,
			AnimationCurve curve)
		{
			StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, curve));
		}

		public void ColorTransition(TextMeshProUGUI changeThis, Color toThis, float delay, float time)
		{
			StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, defaultCurve));
		}

		public IEnumerator _ColorTransition(TextMeshProUGUI changeThis, Color toThis, float delay, float time,
			AnimationCurve curve)
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;
			Color initColor = changeThis.color;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				changeThis.color = Color.LerpUnclamped(initColor, toThis, rate);
				yield return null;
			}
		}

		public IEnumerator _ColorTransition(TextMeshProUGUI changeThis, Color toThis, float delay, float time)
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;
			Color initColor = changeThis.color;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = defaultCurve.Evaluate(passed / time);

				changeThis.color = Color.LerpUnclamped(initColor, toThis, rate);
				yield return null;
			}
		}

		/*
		    Material Functions
		 */


		public void ChangeColor(Renderer changeThis, Color toThis, float delay = 0f)
		{
			StartCoroutine(_ChangeColor(changeThis, toThis, delay));
		}

		public IEnumerator _ChangeColor(Renderer changeThis, Color toThis, float delay = 0f)
		{
			yield return new WaitForSeconds(delay);

			changeThis.material.color = toThis;
		}

		public void ColorTransition(Renderer changeThis, Color toThis, float delay, float time, AnimationCurve curve)
		{
			StartCoroutine(_ColorTransition(changeThis, toThis, delay, time, curve));
		}

		public IEnumerator _ColorTransition(Renderer changeThis, Color toThis, float delay, float time,
			AnimationCurve curve)
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;
			Color initColor = changeThis.material.color;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				changeThis.material.color = Color.LerpUnclamped(initColor, toThis, rate);
				yield return null;
			}
		}

		public void ChangeMaterial(Renderer changeThis, Material toThis, float delay = 0f)
		{
			StartCoroutine(_ChangeMaterial(changeThis, toThis, delay));
		}

		public IEnumerator _ChangeMaterial(Renderer changeThis, Material toThis, float delay = 0f)
		{
			yield return new WaitForSeconds(delay);

			changeThis.material = toThis;
		}

		public void MaterialTransition(Renderer changeThis, Material toThis, float delay, float time,
			AnimationCurve curve)
		{
			StartCoroutine(_MaterialTransition(changeThis, toThis, delay, time, curve));
		}

		public IEnumerator _MaterialTransition(Renderer changeThis, Material toThis, float delay, float time,
			AnimationCurve curve)
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;
			Material initMat = changeThis.material;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				changeThis.material.Lerp(initMat, toThis, rate);
				yield return null;
			}
		}

		public void CanvasGroupAlpha(Transform canvasObject, float targetAlpha, float delay, float time,
			AnimationCurve curve)
		{
			StartCoroutine(_CanvasGroupAlpha(canvasObject, targetAlpha, delay, time, curve));
		}


		public IEnumerator _CanvasGroupAlpha(Transform canvasObject, float targetAlpha, float delay, float time,
			AnimationCurve curve)
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;
			float initAlpha = canvasObject.GetComponent<CanvasGroup>().alpha;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				float a = Mathf.Lerp(initAlpha, targetAlpha, rate);
				canvasObject.GetComponent<CanvasGroup>().alpha = a;
				yield return null;
			}
		}
	}
}