using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace App.Helpers
{
	public class ShaderFunctions : MonoBehaviour
	{
		public Material material;
		public SpriteRenderer spriteRenderer;
		public AnimationCurve defaultCurve = new AnimationCurve();
		[Range(0, 20)] public float defaultDelay;
		[Range(0, 20)] public float defaultTime = 1f;

		private void OnValidate()
		{
			if (defaultCurve.length == 0)
				defaultCurve = AnimationCurve.Linear(0, 0, 1, 1);
		}

		private void Awake()
		{
			if (GetComponent<Renderer>() != null && material == null)
				material = GetComponent<Renderer>().material;

			if (GetComponent<SpriteRenderer>() != null && spriteRenderer == null)
				spriteRenderer = GetComponent<SpriteRenderer>();

			if (GetComponent<Image>() != null && material == null)
				material = GetComponent<Image>().material;
		}

		#region Sprite

		/// <summary>
		/// Changes this sprite's opacity with default transition settings
		/// </summary>
		/// <param name="newOpacity"></param>
		public void ChangeSpriteOpacity(float newOpacity)
		{
			ChangeSpriteOpacity(spriteRenderer, newOpacity, defaultDelay, defaultTime, defaultCurve);
		}

		/// <summary>
		/// Changes given sprite's opacity
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newOpacity"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		public void ChangeSpriteOpacity(SpriteRenderer targetRenderer, float newOpacity, float delay, float time,
			AnimationCurve curve)
		{
			if (targetRenderer == null) return;
			var color = targetRenderer.color;
			Color newColor = new Color(color.r, color.g, color.b,
				newOpacity);
			StartCoroutine(SpriteColorTransition(targetRenderer, newColor, delay, time, curve));
		}

		/// <summary>
		/// Processes the color transition to given newColor for given targetMaterial's given shaderKey with delay, time and curve parameters
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newColor"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <returns></returns>
		public IEnumerator SpriteColorTransition(SpriteRenderer targetRenderer, Color newColor, float delay, float time,
			AnimationCurve curve)
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;
			Color initColor = targetRenderer.color;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				targetRenderer.color = Color.LerpUnclamped(initColor, newColor, rate);
				yield return null;
			}
		}

		#endregion

		#region Opacity

		/// <summary>
		/// Changes this material's opacity with default transition settings
		/// </summary>
		/// <param name="newOpacity"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOpacity(float newOpacity, string shaderKey = "_Background")
		{
			ChangeOpacity(material, newOpacity, defaultDelay, defaultTime, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes this material's opacity with default transition curve
		/// </summary>
		/// <param name="newOpacity"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOpacity(float newOpacity, float delay, float time, string shaderKey = "_Background")
		{
			ChangeOpacity(material, newOpacity, delay, time, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes this material's opacity
		/// </summary>
		/// <param name="newOpacity"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOpacity(float newOpacity, float delay, float time, AnimationCurve curve,
			string shaderKey = "_Background")
		{
			ChangeOpacity(material, newOpacity, delay, time, curve, shaderKey);
		}

		/// <summary>
		/// Changes given Renderer's material opacity with default transition settings
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newOpacity"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOpacity(Renderer targetRenderer, float newOpacity, string shaderKey = "_Background")
		{
			ChangeOpacity(targetRenderer.material, newOpacity, defaultDelay, defaultTime, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes given Renderer's material opacity with default transition curve
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newOpacity"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOpacity(Renderer targetRenderer, float newOpacity, float delay, float time,
			string shaderKey = "_Background")
		{
			ChangeOpacity(targetRenderer.material, newOpacity, delay, time, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes given Renderer's material opacity
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newOpacity"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOpacity(Renderer targetRenderer, float newOpacity, float delay, float time,
			AnimationCurve curve, string shaderKey = "_Background")
		{
			ChangeOpacity(targetRenderer.material, newOpacity, delay, time, curve, shaderKey);
		}

		/// <summary>
		/// Changes given material's opacity
		/// </summary>
		/// <param name="targetMaterial"></param>
		/// <param name="newOpacity"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOpacity(Material targetMaterial, float newOpacity, float delay, float time,
			AnimationCurve curve, string shaderKey = "_Background")
		{
			if (targetMaterial == null) return;
			Color newColor = new Color(targetMaterial.GetColor(shaderKey).r, targetMaterial.GetColor(shaderKey).g,
				targetMaterial.GetColor(shaderKey).b, newOpacity);
			StartCoroutine(ColorTransition(targetMaterial, newColor, delay, time, curve, shaderKey));
		}

		/// <summary>
		/// Changes given material's opacity
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newOpacity"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOpacityInstant(Renderer targetRenderer, float newOpacity, string shaderKey = "_Background")
		{
			if (targetRenderer == null) return;
			Color newColor = new Color(targetRenderer.material.GetColor(shaderKey).r,
				targetRenderer.material.GetColor(shaderKey).g, targetRenderer.material.GetColor(shaderKey).b,
				newOpacity);
			targetRenderer.material.SetColor(shaderKey, newColor);
		}

		/// <summary>
		/// Changes the material's opacity
		/// </summary>
		/// <param name="newOpacity"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOpacityInstant(float newOpacity, string shaderKey = "_Background")
		{
			if (material == null) return;
			Color newColor = new Color(material.GetColor(shaderKey).r, material.GetColor(shaderKey).g,
				material.GetColor(shaderKey).b, newOpacity);
			material.SetColor(shaderKey, newColor);
		}

		#endregion


		#region Color

		/// <summary>
		/// Changes this material's color with default transition settings
		/// </summary>
		/// <param name="newColor"></param>
		/// <param name="shaderKey"></param>
		public void ChangeColor(Color newColor, string shaderKey = "_Background")
		{
			ChangeColor(material, newColor, defaultDelay, defaultTime, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes this material's color with default transition curve
		/// </summary>
		/// <param name="newColor"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="shaderKey"></param>
		public void ChangeColor(Color newColor, float delay, float time, string shaderKey = "_Background")
		{
			ChangeColor(material, newColor, delay, time, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes this material's color
		/// </summary>
		/// <param name="newColor"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		public void ChangeColor(Color newColor, float delay, float time, AnimationCurve curve,
			string shaderKey = "_Background")
		{
			ChangeColor(material, newColor, delay, time, curve, shaderKey);
		}

		/// <summary>
		/// Changes given Renderer's material color with default transition rules
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newColor"></param>
		/// <param name="shaderKey"></param>
		public void ChangeColor(Renderer targetRenderer, Color newColor, string shaderKey = "_Background")
		{
			ChangeColor(targetRenderer.material, newColor, defaultDelay, defaultTime, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes given Renderer's material color with default transition curve
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newColor"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="shaderKey"></param>
		public void ChangeColor(Renderer targetRenderer, Color newColor, float delay, float time,
			string shaderKey = "_Background")
		{
			ChangeColor(targetRenderer.material, newColor, delay, time, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes given Renderer's material color
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newColor"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		public void ChangeColor(Renderer targetRenderer, Color newColor, float delay, float time, AnimationCurve curve,
			string shaderKey = "_Background")
		{
			ChangeColor(targetRenderer.material, newColor, delay, time, curve, shaderKey);
		}

		/// <summary>
		/// Changes given material's color
		/// </summary>
		/// <param name="targetMaterial"></param>
		/// <param name="newColor"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		public void ChangeColor(Material targetMaterial, Color newColor, float delay, float time, AnimationCurve curve,
			string shaderKey = "_Background")
		{
			if (targetMaterial == null) return;
			StartCoroutine(ColorTransition(targetMaterial, newColor, delay, time, curve, shaderKey));
		}

		/// <summary>
		/// Changes the material's color instantly
		/// </summary>
		/// <param name="newColor"></param>
		/// <param name="shaderKey"></param>
		public void ChangeColorInstant(Color newColor, string shaderKey = "_Background")
		{
			if (material == null) return;
			material.SetColor(shaderKey, newColor);
		}

		#endregion


		#region Float, Integer

		/// <summary>
		/// Changes this material's any float/int value with default transition settings
		/// </summary>
		/// <param name="newFloat"></param>
		/// <param name="shaderKey"></param>
		public void ChangeFloat(float newFloat, string shaderKey = "_MainScale")
		{
			ChangeFloat(material, newFloat, defaultDelay, defaultTime, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes this material's any float/int value with default transition curve
		/// </summary>
		/// <param name="newFloat"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="shaderKey"></param>
		public void ChangeFloat(float newFloat, float delay, float time, string shaderKey = "_MainScale")
		{
			ChangeFloat(material, newFloat, delay, time, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes this material's any float/int value
		/// </summary>
		/// <param name="newFloat"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		public void ChangeFloat(float newFloat, float delay, float time, AnimationCurve curve,
			string shaderKey = "_MainScale")
		{
			ChangeFloat(material, newFloat, delay, time, curve, shaderKey);
		}

		/// <summary>
		/// Changes given Renderer's material any float/int value with default transition rules
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newFloat"></param>
		/// <param name="shaderKey"></param>
		public void ChangeFloat(Renderer targetRenderer, float newFloat, string shaderKey = "_MainScale")
		{
			ChangeFloat(targetRenderer.material, newFloat, defaultDelay, defaultTime, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes given Renderer's material any float/int value with default transition curve
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newFloat"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="shaderKey"></param>
		public void ChangeFloat(Renderer targetRenderer, float newFloat, float delay, float time,
			string shaderKey = "_MainScale")
		{
			ChangeFloat(targetRenderer.material, newFloat, delay, time, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes given Renderer's material any float/int value
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newFloat"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		public void ChangeFloat(Renderer targetRenderer, float newFloat, float delay, float time, AnimationCurve curve,
			string shaderKey = "_MainScale")
		{
			ChangeFloat(targetRenderer.material, newFloat, delay, time, curve, shaderKey);
		}

		/// <summary>
		/// Changes given material's any float/int value
		/// </summary>
		/// <param name="targetMaterial"></param>
		/// <param name="newFloat"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		public void ChangeFloat(Material targetMaterial, float newFloat, float delay, float time, AnimationCurve curve,
			string shaderKey = "_MainScale")
		{
			if (targetMaterial == null) return;
			StartCoroutine(FloatTransition(targetMaterial, newFloat, delay, time, curve, shaderKey));
		}

		/// <summary>
		/// Changes the material's any float/int value instantly
		/// </summary>
		/// <param name="newFloat"></param>
		/// <param name="shaderKey"></param>
		public void ChangeFloatInstant(float newFloat, string shaderKey = "_MainScale")
		{
			if (material == null) return;
			material.SetFloat(shaderKey, newFloat);
		}

		#endregion


		#region Offset

		/// <summary>
		/// Changes this material's any offset value with default transition settings
		/// </summary>
		/// <param name="newOffset"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOffset(Vector2 newOffset, string shaderKey = "_MainTex")
		{
			ChangeOffset(material, newOffset, defaultDelay, defaultTime, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes this material's any offset value with default transition curve
		/// </summary>
		/// <param name="newOffset"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOffset(Vector2 newOffset, float delay, float time, string shaderKey = "_MainTex")
		{
			ChangeOffset(material, newOffset, delay, time, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes this material's any offset value
		/// </summary>
		/// <param name="newOffset"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOffset(Vector2 newOffset, float delay, float time, AnimationCurve curve,
			string shaderKey = "_MainTex")
		{
			ChangeOffset(material, newOffset, delay, time, curve, shaderKey);
		}

		/// <summary>
		/// Changes given Renderer's material any offset value with default transition rules
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newOffset"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOffset(Renderer targetRenderer, Vector2 newOffset, string shaderKey = "_MainTex")
		{
			ChangeOffset(targetRenderer.material, newOffset, defaultDelay, defaultTime, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes given Renderer's material any offset value with default transition curve
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newOffset"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOffset(Renderer targetRenderer, Vector2 newOffset, float delay, float time,
			string shaderKey = "_MainTex")
		{
			ChangeOffset(targetRenderer.material, newOffset, delay, time, defaultCurve, shaderKey);
		}

		/// <summary>
		/// Changes given Renderer's material any offset value
		/// </summary>
		/// <param name="targetRenderer"></param>
		/// <param name="newOffset"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOffset(Renderer targetRenderer, Vector2 newOffset, float delay, float time,
			AnimationCurve curve, string shaderKey = "_MainTex")
		{
			ChangeOffset(targetRenderer.material, newOffset, delay, time, curve, shaderKey);
		}

		/// <summary>
		/// Changes given material's any offset value
		/// </summary>
		/// <param name="targetMaterial"></param>
		/// <param name="newOffset"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOffset(Material targetMaterial, Vector2 newOffset, float delay, float time,
			AnimationCurve curve, string shaderKey = "_MainTex")
		{
			if (targetMaterial == null) return;
			StartCoroutine(OffsetTransition(targetMaterial, newOffset, delay, time, curve, shaderKey));
		}

		/// <summary>
		/// Changes the material's any offset value instantly
		/// </summary>
		/// <param name="newOffset"></param>
		/// <param name="shaderKey"></param>
		public void ChangeOffsetInstant(Vector2 newOffset, string shaderKey = "_MainTex")
		{
			if (material == null) return;
			material.SetTextureOffset(shaderKey, newOffset);
		}

		#endregion


		#region Coroutines

		/// <summary>
		/// Processes the color transition to given newColor for given targetMaterial's given shaderKey with delay, time and curve parameters
		/// </summary>
		/// <param name="targetMaterial"></param>
		/// <param name="newColor"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		/// <returns></returns>
		public IEnumerator ColorTransition(Material targetMaterial, Color newColor, float delay, float time,
			AnimationCurve curve, string shaderKey = "_Background")
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;
			Color initColor = targetMaterial.GetColor(shaderKey);

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				targetMaterial.SetColor(shaderKey, Color.LerpUnclamped(initColor, newColor, rate));
				yield return null;
			}
		}

		/// <summary>
		/// Processes the any float/int transition to given newFloat for given targetMaterial's given shaderKey with delay, time and curve parameters
		/// </summary>
		/// <param name="targetMaterial"></param>
		/// <param name="newFloat"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		/// <returns></returns>
		public IEnumerator FloatTransition(Material targetMaterial, float newFloat, float delay, float time,
			AnimationCurve curve, string shaderKey = "_MainScale")
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;
			float initFloat = targetMaterial.GetFloat(shaderKey);

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				targetMaterial.SetFloat(shaderKey, Mathf.LerpUnclamped(initFloat, newFloat, rate));
				yield return null;
			}
		}

		/// <summary>
		/// Processes the any offset transition to given newOffset for given targetMaterial's given shaderKey with delay, time and curve parameters
		/// </summary>
		/// <param name="targetMaterial"></param>
		/// <param name="newOffset"></param>
		/// <param name="delay"></param>
		/// <param name="time"></param>
		/// <param name="curve"></param>
		/// <param name="shaderKey"></param>
		/// <returns></returns>
		public IEnumerator OffsetTransition(Material targetMaterial, Vector2 newOffset, float delay, float time,
			AnimationCurve curve, string shaderKey = "_MainTex")
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;
			Vector2 initOffset = targetMaterial.GetTextureOffset(shaderKey);

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				targetMaterial.SetTextureOffset(shaderKey, Vector2.LerpUnclamped(initOffset, newOffset, rate));
				yield return null;
			}
		}

		#endregion

		public float GetFloat(string shaderKey)
		{
			return material.GetFloat(shaderKey);
		}
	}
}