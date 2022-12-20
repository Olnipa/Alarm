using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

[RequireComponent(typeof(AudioSource))]

public class AlarmSoundPlayer : MonoBehaviour
{
    [SerializeField] private ThiefDetector _thiefDetector;

    private AudioSource _audioSource;
    private float maxVolume = 1.0f;
    private float minVolume = 0;
    private int _framesToChangeAlarmVolume = 1000;
    private Coroutine _alarmVolumeUpJob;
    private Coroutine _alarmVolumeDownJob;

    private void Start()
    {
        _thiefDetector.ThiefSpotted += PlayAlarmSound;
        _thiefDetector.ThiefIsGone += StopAlarmSound;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
        _audioSource.loop = true;
    }

    private IEnumerator ChangeAlarmVolume(float targetVolume)
    {
        for (int i = 0; i < _framesToChangeAlarmVolume; i++)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetVolume, maxVolume / _framesToChangeAlarmVolume);


            yield return null;
        }

        if (_audioSource.volume == 0)
        {
            _audioSource.Stop();
        }
    }

    private void PlayAlarmSound()
    {
        if(_audioSource.isPlaying == false)
        {
            _audioSource.Play();
        }

        if (_alarmVolumeDownJob != null)
            StopCoroutine(_alarmVolumeDownJob);

        _alarmVolumeUpJob = StartCoroutine(ChangeAlarmVolume(maxVolume));
    }

    private void StopAlarmSound()
    {
        if (_alarmVolumeUpJob != null)
            StopCoroutine(_alarmVolumeUpJob);

        _alarmVolumeDownJob = StartCoroutine(ChangeAlarmVolume(minVolume));
    }
}
