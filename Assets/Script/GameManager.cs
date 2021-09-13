using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }

		public Vector2 MapSize { get; } = new Vector2(14, 24);

		public bool IsGameWorking { get; set; }
		public float speed;

		public ScorePrint ScorePrint => scorePrint;
		[SerializeField] private ScorePrint scorePrint;

		[SerializeField] private SnakeController snakeController;
		[SerializeField] private Canvas canvasStart;
		[SerializeField] private CanvasHomeAnimation canvasHomeAnimation;
		[SerializeField] private Canvas canvasEnd;
		[SerializeField] private ColorManager colorManager;

		[SerializeField] private Image imageMusicOn;
		[SerializeField] private Image imageMusicOff;

		[SerializeField] private CanvasTutorial canvasTutorial;
		[SerializeField] private FoodController bonusFood;
		[SerializeField] private List<EnemyController> listEnemy;

		[SerializeField] private string URL;

		public void BonusLoop()
		{
			IEnumerator Do()
			{
				yield return new WaitForSeconds(10);
				ChangePositionFood(bonusFood);
				bonusFood.gameObject.SetActive(true);
				bonusFood.OnSpawn();
			}

			StartCoroutine(Do());
		}


		public void ChangePositionFood(FoodController foodController)
		{
			Vector2 newFoodPosition;
			do
			{
				var areaLimit = GameManager.Instance.MapSize;
				var x = (int) Random.Range(1, areaLimit.x);
				var y = (int) Random.Range(1, areaLimit.y);
				newFoodPosition = new Vector2(x, y);
			} while (!GameManager.Instance.IsEmpty(newFoodPosition));


			foodController.transform.position = newFoodPosition;
		}

		public int Score
		{
			get => _score;
			set
			{
				var old = _score / 10;

				_score = value;
				scorePrint.Print(value);

				var newValue = _score / 10;

				if (_score % 10 == 0 || newValue - old >= 1)
				{
					if (_score != 0) AudioManager.Instance.Play(ClipType.LevelUp);
					colorManager.RandomChange();
					if (_score != 0 && _enemyIndex < listEnemy.Count)
					{
						listEnemy[_enemyIndex].MyStart();
						_enemyIndex++;
					}
				}
			}
		}

		private int _enemyIndex;
		private int _score;

		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			Score = 0;

			Application.targetFrameRate = 60;
		}

		public void TryAgain()
		{
			foreach (var enemy in listEnemy)
				enemy.Stop();

			scorePrint.ResetPrint();
			snakeController.ResetSnake();
			canvasEnd.gameObject.SetActive(false);
			IsGameWorking = true;
			Score = 0;
			BonusLoop();
		}

		public bool IsEmpty(Vector2 newPosition)
		{
			int newPositionX = Mathf.RoundToInt(newPosition.x);
			int newPositionY = Mathf.RoundToInt(newPosition.y);

			foreach (var item in snakeController.Snake)
			{
				var position = item.position;
				var x = Mathf.RoundToInt(position.x);
				var y = Mathf.RoundToInt(position.y);

				if (x == newPositionX && y == newPositionY)
					return false;
			}

			foreach (var item in ScorePrint.listExceptPosition)
			{
				var position = item;
				var x = Mathf.RoundToInt(position.x);
				var y = Mathf.RoundToInt(position.y);
				if (x == newPositionX && y == newPositionY)
					return false;
			}

			return true;
		}

		public void StartGame()
		{
			canvasTutorial.Open();
			IsGameWorking = true;
			snakeController.MyStart();
			canvasStart.gameObject.SetActive(false);
			canvasHomeAnimation.StopAllCoroutines();
			BonusLoop();
		}

		public void GameOver()
		{
			if (!IsGameWorking)
				return;

			AudioManager.Instance.Play(ClipType.Dead);
			AudioManager.Instance.UpdateMusicPitch(false);
			IsGameWorking = false;
			canvasEnd.gameObject.SetActive(true);

			StopAllCoroutines();
			bonusFood.gameObject.SetActive(false);
			bonusFood.StopAllCoroutines();
			_enemyIndex = 0;
		}

		public void OnClick_Music()
		{
			AudioManager.Instance.ToggleMusic();

			imageMusicOff.enabled = !imageMusicOff.enabled;
			imageMusicOn.enabled = !imageMusicOn.enabled;
		}

		public void OnClick_Comment()
		{
			Application.OpenURL(URL);
		}
	}
}