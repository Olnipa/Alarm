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
    private bool _alarmSoundIsOn;
    private int _framesToChangeAlarmVolume = 1000;
    private Coroutine _alarmVolumeUpJob;
    private Coroutine _alarmVolumeDownJob;

    private void Start()
    {
        _thiefDetector.ThiefSpotted += PlayAlarmSound;
        _thiefDetector.ThiefIsGone += StopAlarmSound;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
    }

    private IEnumerator ChangeAlarmVolume(float targetVolume)
    {
        for (int i = 0; i < _framesToChangeAlarmVolume; i++)
        {
            if (_alarmSoundIsOn)
            {
                _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetVolume, maxVolume / _framesToChangeAlarmVolume);
            }

            if (_audioSource.volume == 0)
            {
                _audioSource.Stop();
            }

            yield return null;
        }
    }

    private void PlayAlarmSound()
    {
        _audioSource.loop = true;
        _alarmSoundIsOn = true;

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
        _alarmSoundIsOn = false;

        if (_alarmVolumeUpJob != null)
            StopCoroutine(_alarmVolumeUpJob);

        _alarmVolumeDownJob = StartCoroutine(ChangeAlarmVolume(minVolume));
    }
}
