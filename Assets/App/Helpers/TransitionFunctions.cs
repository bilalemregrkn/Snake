using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace App.Helpers
{
	public class TransitionFunctions : MonoBehaviour
	{
		[SerializeField] private AnimationCurve curveDefault = new AnimationCurve();

		private void OnValidate()
		{
			if (curveDefault.length == 0)
				curveDefault = AnimationCurve.EaseInOut(0, 0, 1, 1);
		}

		#region Queue

		public List<IEnumerator> listIEnumerator = new List<IEnumerator>();

		public void AddQueue(IEnumerator function)
		{
			listIEnumerator.Add(function);

			if (listIEnumerator.Count == 1)
				StartCoroutine(ListProcess());
		}

		public void ListInsert(int index, IEnumerator function)
		{
			listIEnumerator.Insert(index, function);
		}

		public void AddSubQueue(IEnumerator[] functions)
		{
			List<IEnumerator> list = new List<IEnumerator>();
			foreach (var item in functions)
			{
				list.Add(item);
			}

			AddSubQueue(list);
		}

		private void AddSubQueue(List<IEnumerator> functionList)
		{
			functionList.Reverse();

			int index = 0;
			if (listIEnumerator.Count != 0)
			{
				index = 1;
			}

			foreach (var item in functionList)
			{
				listIEnumerator.Insert(index, item);
			}

			if (index == 0)
			{
				StartCoroutine(ListProcess());
			}
		}

		private IEnumerator ListProcess()
		{
			while (listIEnumerator.Count > 0)
			{
				yield return StartCoroutine(listIEnumerator[0]);

				if (listIEnumerator.Count > 0)
					listIEnumerator.RemoveAt(0);
			}
		}

		#endregion

		#region Position Functions

		public void Teleport(Transform teleportThis, Transform toThis, float delay = 0f)
		{
			StartCoroutine(_Teleport(teleportThis, toThis, delay));
		}

		private IEnumerator _Teleport(Transform teleportThis, Transform toThis, float delay = 0f)
		{
			if (delay > 0)
				yield return new WaitForSeconds(delay);

			teleportThis.position = toThis.position;
		}

		public void Teleport(Transform teleportThis, Vector3 toThis, float delay = 0f)
		{
			StartCoroutine(_Teleport(teleportThis, toThis, delay));
		}

		private IEnumerator _Teleport(Transform teleportThis, Vector3 toThis, float delay = 0f)
		{
			if (delay > 0)
				yield return new WaitForSeconds(delay);

			teleportThis.position = toThis;
		}

		public void Move(Transform moveThis, Transform toThis, float delay = 0, float time = 1,
			[CanBeNull] AnimationCurve curve = null)
		{
			StartCoroutine(_Move(moveThis, toThis, delay, time, curve ?? curveDefault));
		}

		public void Move(Transform moveThis, Vector3 toThis, float delay = 0, float time = 1,
			[CanBeNull] AnimationCurve curve = null)
		{
			StartCoroutine(_Move(moveThis, toThis, delay, time, curve ?? curveDefault));
		}

		private IEnumerator _Move(Transform moveThis, Vector3 toThis, float delay, float time, AnimationCurve curve)
		{
			if (delay > 0)
				yield return new WaitForSeconds(delay);
			float passed = 0f;
			Vector3 initPos = moveThis.position;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				moveThis.position = Vector3.LerpUnclamped(initPos, toThis, rate);
				yield return null;
			}
		}

		private IEnumerator _Move(Transform moveThis, Transform toThis, float delay, float time, AnimationCurve curve)
		{
			if (delay > 0)
				yield return new WaitForSeconds(delay);
			float passed = 0f;
			Vector3 initPos = moveThis.position;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				moveThis.position = Vector3.LerpUnclamped(initPos, toThis.position, rate);
				yield return null;
			}
		}

		#endregion

		#region Position UI -Rect Transform- Functions

		public void TeleportUI(RectTransform moveThis, RectTransform toThis, float delay = 0)
		{
			StartCoroutine(_TeleportUI(moveThis, toThis, delay));
		}

		public IEnumerator _TeleportUI(RectTransform moveThis, RectTransform toThis, float delay)
		{
			if (delay > 0) yield return new WaitForSeconds(delay);
			ChangeAnchor(moveThis, toThis);
			moveThis.localPosition = toThis.localPosition;
		}

		public void MoveUI(RectTransform moveThis, RectTransform toThis, float delay = 0, float time = 1,
			[CanBeNull] AnimationCurve curve = null)
		{
			StartCoroutine(_MoveUI(moveThis, toThis, delay, time, curve ?? curveDefault));
		}

		public void MoveUI(RectTransform moveThis, Vector3 toThis, float delay = 0, float time = 1,
			[CanBeNull] AnimationCurve curve = null)
		{
			StartCoroutine(_MoveUI(moveThis, toThis, delay, time, curve ?? curveDefault));
		}

		private IEnumerator _MoveUI(RectTransform moveThis, RectTransform toThis, float delay, float time,
			AnimationCurve curve)
		{
			if (delay > 0) yield return new WaitForSeconds(delay);
			ChangeAnchor(moveThis, toThis);
			float passed = 0f;
			Vector3 initPos = moveThis.localPosition;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				moveThis.localPosition = Vector3.LerpUnclamped(initPos, toThis.localPosition, rate);
				yield return null;
			}
		}

		private IEnumerator _MoveUI(RectTransform moveThis, Vector3 toThis, float delay, float time,
			AnimationCurve curve)
		{
			if (delay > 0) yield return new WaitForSeconds(delay);
			float passed = 0f;
			Vector3 initPos = moveThis.localPosition;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				moveThis.localPosition = Vector3.LerpUnclamped(initPos, toThis, rate);
				yield return null;
			}
		}

		private void ChangeAnchor(RectTransform current, RectTransform target)
		{
			ChangePivot(current, target);

			var currentTransform = current.transform;
			Vector3 initialPos = currentTransform.position;
			current.anchorMin = target.anchorMin;
			current.anchorMax = target.anchorMax;
			currentTransform.position = initialPos;
		}

		private void ChangePivot(RectTransform current, RectTransform target)
		{
			var currentRect = current.rect;
			float width = currentRect.width; //Get width Current object 
			float height = currentRect.height; //Get height Current object 

			Vector2 initialAnchoredPosition = current.anchoredPosition; //Get initial position
			var pivot = target.pivot;
			Vector2 pivotDistance = pivot - current.pivot; //Dönüşecegi target'in pivotları arası fark bulunur.


			//Pivot noktası değiştiği zaman achored pos sabit kaldığı için objenin konumu değişiyor gibi görünüyor.
			current.pivot = pivot;

			//Objenin width & height'ına göre ufak bir konum değişikligi yapmalıyız.
			Vector2 offsetPos = new Vector2(0, 0);

			offsetPos.x = width * pivotDistance.x; //pivot farklarına göre oranlar bulunur ve offset konum bulunur.
			offsetPos.y = height * pivotDistance.y;

			current.anchoredPosition =
				initialAnchoredPosition + offsetPos; //İlk konumuna offset eklenince obje hiç kıpırdamamış olur.

			/*Böylece pivot noktası değişse dahi obje aynı şekilde kalacak.*/
		}

		#endregion

		#region Rotation Functions

		public void ChangeRotation(Transform changeThis, Transform toThis, float delay = 0f)
		{
			StartCoroutine(_ChangeRotation(changeThis, toThis, delay));
		}

		private IEnumerator _ChangeRotation(Transform changeThis, Transform toThis, float delay = 0f)
		{
			if (delay > 0) yield return new WaitForSeconds(delay);

			changeThis.rotation = toThis.rotation;
		}

		public void Rotate(Transform rotateThis, Transform toThis, float delay = 0, float time = 1,
			[CanBeNull] AnimationCurve curve = null)
		{
			StartCoroutine(_Rotate(rotateThis, toThis, delay, time, curve ?? curveDefault));
		}

		private IEnumerator _Rotate(Transform rotateThis, Transform toThis, float delay, float time,
			AnimationCurve curve)
		{
			if (delay > 0) yield return new WaitForSeconds(delay);
			float passed = 0f;
			Quaternion initRot = rotateThis.rotation;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				rotateThis.rotation = Quaternion.LerpUnclamped(initRot, toThis.rotation, rate);
				yield return null;
			}
		}

		#endregion

		#region Scale Functions

		public void ChangeScale(Transform changeThis, Transform toThis, float delay = 0f)
		{
			StartCoroutine(_ChangeScale(changeThis, toThis, delay));
		}

		private IEnumerator _ChangeScale(Transform changeThis, Transform toThis, float delay = 0f)
		{
			if (delay > 0)
				yield return new WaitForSeconds(delay);

			changeThis.localScale = toThis.localScale;
		}

		public void Scale(Transform scaleThis, Transform toThis, float delay = 0, float time = 1,
			[CanBeNull] AnimationCurve curve = null)
		{
			StartCoroutine(_Scale(scaleThis, toThis, delay, time, curve ?? curveDefault));
		}

		public void Scale(Transform scaleThis, Vector3 toThis, float delay = 0, float time = 1,
			[CanBeNull] AnimationCurve curve = null)
		{
			StartCoroutine(_Scale(scaleThis, toThis, delay, time, curve ?? curveDefault));
		}

		public IEnumerator _Scale(Transform scaleThis, Vector3 toThis, float delay, float time, AnimationCurve curve)
		{
			if (curve == null)
				curve = curveDefault;

			if (delay > 0)
				yield return new WaitForSeconds(delay);

			float passed = 0f;
			Vector3 initScale = scaleThis.localScale;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				scaleThis.localScale = Vector3.LerpUnclamped(initScale, toThis, rate);
				yield return null;
			}
		}

		public IEnumerator _Scale(Transform scaleThis, Transform toThis, float delay, float time, AnimationCurve curve)
		{
			if (curve == null)
				curve = curveDefault;

			if (delay > 0)
				yield return new WaitForSeconds(delay);

			float passed = 0f;
			Vector3 initScale = scaleThis.localScale;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				scaleThis.localScale = Vector3.LerpUnclamped(initScale, toThis.localScale, rate);
				yield return null;
			}
		}


		public void ChangeTextFont(TextMeshProUGUI scaleThis, float toThis, float delay = 0, float time = 1,
			[CanBeNull] AnimationCurve curve = null)
		{
			StartCoroutine(_ChangeTextFont(scaleThis, toThis, delay, time, curve ?? curveDefault));
		}

		private IEnumerator _ChangeTextFont(TextMeshProUGUI scaleThis, float toThis, float delay, float time,
			AnimationCurve curve)
		{
			if (delay > 0) yield return new WaitForSeconds(delay);
			float passed = 0f;
			float initFontSize = scaleThis.fontSize;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				scaleThis.fontSize = Mathf.LerpUnclamped(initFontSize, toThis, rate);
				yield return null;
			}
		}

		#endregion

		#region Parent Functions

		public void SetParentAfter(Transform thisIsChild, Transform thisIsParent, float delay = 0f)
		{
			StartCoroutine(_SetParentAfter(thisIsChild, thisIsParent, delay));
		}

		private IEnumerator _SetParentAfter(Transform thisIsChild, Transform thisIsParent, float delay)
		{
			if (delay > 0) yield return new WaitForSeconds(delay);
			thisIsChild.SetParent(thisIsParent);
		}

		#endregion

		#region GameObject/Component Functions

		public void SetActiveAfter(GameObject activateThis, bool isActive = true, float delay = 0f)
		{
			StartCoroutine(_SetActiveAfter(activateThis, isActive, delay));
		}

		private IEnumerator _SetActiveAfter(GameObject activateThis, bool isActive, float delay)
		{
			if (delay > 0) yield return new WaitForSeconds(delay);
			activateThis.SetActive(isActive);
		}


		public void SetEnabledAfter<T>(GameObject go, bool isActive, float delay = 0f) where T : Component
		{
			StartCoroutine(_SetEnabledAfter<T>(go, isActive, delay));
		}

		IEnumerator _SetEnabledAfter<T>(GameObject go, bool isActive, float delay = 0f) where T : Component
		{
			var component = go.GetComponent<T>();
			yield return new WaitForSeconds(delay);

			if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
			{
				if (component is MonoBehaviour { } current) current.enabled = isActive;
			}
		}

		#endregion

		#region Material Function

		public void ChangeMaterial(Transform current, Material toMaterial, float delay = 0f)
		{
			StartCoroutine(_ChangeMaterial(current, toMaterial, delay));
		}

		public IEnumerator _ChangeMaterial(Transform current, Material toMaterial, float delay = 0f)
		{
			yield return new WaitForSeconds(delay);

			current.GetComponent<MeshRenderer>().material = toMaterial;
		}

		public void MaterialLerp(Transform materialThis, Material toMaterial, float delay, float time,
			AnimationCurve curve)
		{
			StartCoroutine(_MaterialLerp(materialThis, toMaterial, delay, time, curve));
		}

		public IEnumerator _MaterialLerp(Transform materialThis, Material toMaterial, float delay, float time,
			AnimationCurve curve)
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;
			Material initMat = materialThis.GetComponent<MeshRenderer>().material;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				initMat.Lerp(initMat, toMaterial, rate); //BURASI HATALI!!!!
				yield return null;
			}
		}

		#endregion

		#region Sound Function

		public void ChangeVolume(AudioSource volumeThis, float toVolume = 1f, float delay = 0f)
		{
			StartCoroutine(_ChangeVolume(volumeThis, toVolume, delay));
		}

		public IEnumerator _ChangeVolume(AudioSource volumeThis, float toVolume, float delay = 0f)
		{
			yield return new WaitForSeconds(delay);

			volumeThis.volume = toVolume;
		}

		public void Volume(AudioSource volumeThis, float toVolume, float delay, float time, AnimationCurve curve)
		{
			StartCoroutine(_Volume(volumeThis, toVolume, delay, time, curve));
		}

		public IEnumerator _Volume(AudioSource volumeThis, float toVolume, float delay, float time,
			AnimationCurve curve)
		{
			yield return new WaitForSeconds(delay);
			float passed = 0f;
			float initVolume = volumeThis.volume;

			while (passed < time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				volumeThis.volume = Mathf.LerpUnclamped(initVolume, toVolume, rate);
				yield return null;
			}
		}

		#endregion

		#region Number Change

		public void NumberChange(TextMeshProUGUI textNumber, float targetNumber, float delay, float time,
			AnimationCurve curve)
		{
			StartCoroutine(_NumberChange(textNumber, targetNumber, delay, time, curve));
		}

		public void NumberChange(TextMeshProUGUI textNumber, float targetNumber, float delay, float time)
		{
			StartCoroutine(_NumberChange(textNumber, targetNumber, delay, time, curveDefault));
		}

		public IEnumerator _NumberChange(TextMeshProUGUI textNumber, float targetNumber, float delay, float time,
			AnimationCurve curve)
		{
			yield return new WaitForSeconds(delay);

			float passed = 0;
			float initialNumber = System.Convert.ToInt32(textNumber.text);

			while (passed <= time)
			{
				passed += Time.deltaTime;
				var rate = curve.Evaluate(passed / time);

				textNumber.text = Mathf.Lerp(initialNumber, targetNumber, rate).ToString("0");
				yield return null;
			}
		}

		#endregion
	}
}