using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
	private AudioSource _mySource;

	public static AudioManager Instance { get; private set; }

	[SerializeField] private AudioClip buttonClick;
	[SerializeField] private AudioClip snakeEat;
	[SerializeField] private AudioClip snakeDead;
	[SerializeField] private AudioClip levelUp;

	[SerializeField] private AudioMixerSnapshot onSnapshot;
	[SerializeField] private AudioMixerSnapshot offSnapshot;

	[SerializeField] private AudioSource musicSource;
	[SerializeField,Range(0,2f)] private float pitchOnGame;
	[SerializeField,Range(0,2f)] private float pitchOnMenu;
	[SerializeField] private AudioLowPassFilter lowPassFilter;

	private bool _isMusicOn = true;

	private void Awake()
	{
		Instance = this;

		musicSource.pitch = pitchOnMenu;
	}

	private void Start()
	{
		_mySource = GetComponent<AudioSource>();

		UpdateMusic();
	}

	public void ToggleMusic()
	{
		_isMusicOn = !_isMusicOn;
		UpdateMusic();
	}

	private void UpdateMusic()
	{
		var snapshot = _isMusicOn ? onSnapshot : offSnapshot;
		snapshot.TransitionTo(.2f);
	}

	public void Play(ClipType type, float volume = 1)
	{
		var clip = GetClip(type);
		Play(clip, volume);
	}

	private void Play(AudioClip clip, float volume = 1)
	{
		_mySource.PlayOneShot(clip, volume);
	}

	private AudioClip GetClip(ClipType type)
	{
		return type switch
		{
			ClipType.Click => buttonClick,
			ClipType.Eat => snakeEat,
			ClipType.Dead => snakeDead,
			ClipType.LevelUp => levelUp,
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
		};
	}

	public void UpdateMusicPitch(bool isGame)
	{
		var target = isGame ? pitchOnGame : pitchOnMenu;
		StartCoroutine(ChangePitch(musicSource, target, .3f));
		lowPassFilter.enabled = !isGame;
	}

	private IEnumerator ChangePitch(AudioSource source, float target, float time)
	{
		float passed = 0;
		var init = source.pitch;
		while (passed < time)
		{
			passed += Time.deltaTime;
			source.pitch = Mathf.Lerp(init, target, passed / time);
			yield return null;
		}
	}
}