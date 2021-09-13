using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script
{
	public class SnakeController : MonoBehaviour
	{
		[SerializeField] private FoodController food;
		[SerializeField] private GameObject tailPrefabs;
		[SerializeField] private Collider2D myCollider;
		[SerializeField] private TrailRenderer myTrailRenderer;
		[SerializeField] private float oneTrailLength;
		[SerializeField] private bool isSmooth;
		[SerializeField] private int testSpeedGrow;
		[SerializeField] private Transform snakeSprite;

		[SerializeField] private List<GameObject> eyeDead;
		[SerializeField] private List<GameObject> eyeAlive;
		[SerializeField] private GameObject lip;
		[SerializeField] private Animator snakeEat;

		private Vector2 _direction = Vector2.down;
		private Vector2 _lastDirection = Vector2.down;

		public List<Transform> Snake => _snake;
		private readonly List<Transform> _snake = new List<Transform>();

		private FoodController _currentFood;
		private bool _grow;
		
		public void MyStart()
		{
			GameManager.Instance.ChangePositionFood(food);
			StartCoroutine(Move());

			_snake.Add(transform);

			UpdateEyeAndLip(true);
			AudioManager.Instance.UpdateMusicPitch(true);
		}

		private void UpdateEyeAndLip(bool isAlive)
		{
			lip.SetActive(!isAlive);

			foreach (var eye in eyeAlive)
				eye.SetActive(isAlive);

			foreach (var eye in eyeDead)
				eye.SetActive(!isAlive);
		}

		private void Update()
		{
			if (!GameManager.Instance.IsGameWorking) return;

			if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && _lastDirection != Vector2.right)
				_direction = Vector2.left;

			if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && _lastDirection != Vector2.left)
				_direction = Vector2.right;

			if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && _lastDirection != Vector2.down)
				_direction = Vector2.up;

			if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && _lastDirection != Vector2.up)
				_direction = Vector2.down;
		}

		private void UpdateRotateSprite(Vector2 direction)
		{
			var rotation = Vector3.zero;
			if (direction == Vector2.down)
				rotation = Vector3.zero;
			if (direction == Vector2.right)
				rotation = new Vector3(0, 0, 90);
			if (direction == Vector2.up)
				rotation = new Vector3(0, 0, 180);
			if (direction == Vector2.left)
				rotation = new Vector3(0, 0, 270);

			snakeSprite.localEulerAngles = rotation;
		}

		private IEnumerator Move()
		{
			yield return new WaitForSeconds(1);
			while (true)
			{
				if (_grow || testSpeedGrow > 0)
				{
					testSpeedGrow--;
					myCollider.enabled = false;
					_grow = false;
					Grow(_currentFood);
					myTrailRenderer.time += oneTrailLength;
				}

				for (int i = _snake.Count - 1; i > 0; i--)
				{
					if (isSmooth)
					{
						var tailPosition = _snake[i - 1].position;
						StartCoroutine(MoveAnimation(_snake[i], tailPosition, GameManager.Instance.speed));
					}
					else
					{
						_snake[i].position = _snake[i - 1].position;
					}
				}

				var position = transform.position;
				position += (Vector3) _direction;
				_lastDirection = (Vector2) _direction;
				position.x = Mathf.RoundToInt(position.x);
				position.y = Mathf.RoundToInt(position.y);

				UpdateRotateSprite(_direction);

				if (isSmooth)
				{
					yield return MoveAnimation(transform, position, GameManager.Instance.speed);
					myCollider.enabled = true;
				}
				else
				{
					transform.position = position;
					yield return new WaitForSeconds(GameManager.Instance.speed);
				}
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

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Food"))
			{
				_currentFood = other.GetComponent<FoodController>();
				_grow = true;
			}

			if (other.CompareTag("Wall"))
			{
				Dead();
			}
		}


		private void Grow(FoodController foodController)
		{
			GameManager.Instance.Score += foodController.score;

			var tail = Instantiate(tailPrefabs);
			_snake.Add(tail.transform);
			_snake[_snake.Count - 1].position = _snake[_snake.Count - 2].position;

			GameManager.Instance.ChangePositionFood(foodController);
			foodController.OnEat();

			AudioManager.Instance.Play(ClipType.Eat);

			if (snakeEat.enabled)
				snakeEat.Play(0);
			else
				snakeEat.enabled = true;
		}

	


		private void Dead()
		{
			myTrailRenderer.time = Mathf.Infinity;
			StopAllCoroutines();
			UpdateEyeAndLip(false);
			GameManager.Instance.GameOver();
		}

		public void ResetSnake()
		{
			_snake.RemoveAt(0);
			foreach (var tail in _snake)
				Destroy(tail.gameObject);

			_snake.Clear();

			transform.position = new Vector3(3, 20, 0);
			_direction = Vector2.down;
			_lastDirection = _direction;
			UpdateRotateSprite(_direction);

			myTrailRenderer.time = 0.1f;
			myTrailRenderer.Clear();

			MyStart();
		}
	}
}